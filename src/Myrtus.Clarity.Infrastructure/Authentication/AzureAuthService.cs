using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions.Authentication;
using MimeKit;
using Myrtus.Clarity.Application.Services.Auth;
using Myrtus.Clarity.Core.Application.Abstractions.Mailing;
using Myrtus.Clarity.Core.Domain.Abstractions.Mailing;
using System.Security.Cryptography;
using System.Text;
using User = Myrtus.Clarity.Domain.Users.User;

namespace Myrtus.Clarity.Infrastructure.Authentication
{
    public sealed class AzureAuthService : IAuthService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly IConfiguration _configuration;
        private readonly IMailService _emailService;
        private readonly ILogger<AzureAuthService> _logger;

        public AzureAuthService(
            IConfiguration configuration,
            ILogger<AzureAuthService> logger,
            IMailService emailService)
        {
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;

            // Azure AD settings
            var clientId = _configuration["AzureAd:ClientId"];
            var clientSecret = _configuration["AzureAd:ClientSecret"];
            var tenantId = _configuration["AzureAd:TenantId"];
            var authority = $"{_configuration["AzureAd:Instance"]}{tenantId}/v2.0";
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            // Build the confidential client application
            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(authority)
                .Build();

            // Create the token provider
            var tokenProvider = new TokenProvider(confidentialClientApplication, scopes);

            // Create the authentication provider
            var authenticationProvider = new BaseBearerTokenAuthenticationProvider(tokenProvider);

            // Initialize GraphServiceClient with the authentication provider
            _graphClient = new GraphServiceClient(authenticationProvider);
        }

        public async Task<string> RegisterAsync(
            User user,
            string password,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Create user object with Azure AD B2C specifics
                var userToCreate = new Microsoft.Graph.Models.User
                {
                    AccountEnabled = true,
                    DisplayName = $"{user.FirstName.Value} {user.LastName.Value}",
                    GivenName = user.FirstName.Value,
                    Surname = user.LastName.Value,
                    Identities = new List<ObjectIdentity>
                        {
                            new ObjectIdentity
                            {
                                SignInType = "emailAddress",
                                Issuer = _configuration["AzureAd:Domain"],
                                IssuerAssignedId = user.Email.Value
                            }
                        },
                    PasswordProfile = new PasswordProfile
                    {
                        Password = password
                    },
                    PasswordPolicies = "DisablePasswordExpiration"
                };

                // Create the user in Azure AD B2C
                var createdUser = await _graphClient.Users
                    .PostAsync(userToCreate, cancellationToken: cancellationToken);

                // Send email to user with link to reset their password
                await SendPasswordResetEmailAsync(user, cancellationToken);

                // Return the user's object ID
                return createdUser.Id;
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "ServiceException: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception: {Message}", ex.Message);
                throw;
            }
        }

        private async Task SendPasswordResetEmailAsync(User user, CancellationToken cancellationToken)
        {
            var codeVerifier = GenerateCodeVerifier();
            var codeChallenge = GenerateCodeChallenge(codeVerifier);

            // Construct the password reset link
            var passwordResetPolicyId = _configuration["AzureAdB2C:PasswordResetPolicyId"];
            var clientId = _configuration["AzureAdB2C:ClientId"];
            var redirectUri = _configuration["AzureAdB2C:RedirectUri"];
            var tenantDomain = _configuration["AzureAdB2C:Domain"];
            var instance = _configuration["AzureAdB2C:Instance"];

            var queryParams = new Dictionary<string, string>
                {
                    {"p", passwordResetPolicyId},
                    {"client_id", clientId},
                    {"nonce", "defaultNonce"},
                    {"redirect_uri", redirectUri},
                    {"scope", "openid"},
                    {"response_type", "code"},
                    {"prompt", "login"},
                    {"login_hint", user.Email.Value},
                    {"code_challenge", codeChallenge},
                    {"code_challenge_method", "S256"}
                };

            var passwordResetLink = QueryHelpers.AddQueryString($"{instance}/{tenantDomain}/oauth2/v2.0/authorize", queryParams);

            // Create the email message using Mail object
            var subject = "Welcome to Myrtus Clarity - Set Your Password";
            var textBody = $"Dear {user.FirstName.Value},\n\n" +
                           "Welcome to Myrtus Clarity! Please click the link below to set your password and activate your account:\n" +
                           $"{passwordResetLink}\n\n" +
                           "If you did not expect this email, please ignore it.\n\n" +
                           "Best regards,\nThe Myrtus Clarity Team";

            var htmlBody = $@"
                    <p>Dear {user.FirstName.Value},</p>
                    <p>Welcome to Myrtus Clarity! Please click the link below to set your password and activate your account:</p>
                    <p><a href=""{passwordResetLink}"">Set your password</a></p>
                    <p>If you did not expect this email, please ignore it.</p>
                    <p>Best regards,<br/>The Myrtus Clarity Team</p>";

            var mail = new Mail(
                subject: subject,
                textBody: textBody,
                htmlBody: htmlBody,
                toList: new List<MailboxAddress>
                {
                        new MailboxAddress(
                            encoding: Encoding.UTF8,
                            name: $"{user.FirstName} {user.LastName}",
                            address: user.Email.Value)
                });

            // Send the email using IMailService
            await _emailService.SendEmailAsync(mail);
        }

        private string GenerateCodeVerifier()
        {
            const int length = 128;
            byte[] bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            return Base64UrlEncode(bytes);
        }

        private string GenerateCodeChallenge(string codeVerifier)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.ASCII.GetBytes(codeVerifier);
            var hash = sha256.ComputeHash(bytes);
            return Base64UrlEncode(hash);
        }

        private string Base64UrlEncode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}
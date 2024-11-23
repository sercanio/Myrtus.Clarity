using Myrtus.CMS.Application.Services.Auth;
using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Infrastructure.Authentication.Models;
using System.Net.Http.Json;

namespace Myrtus.CMS.Infrastructure.Authentication
{
    public sealed class AuthService(HttpClient httpClient) : IAuthService
    {
        private const string PasswordCredentialType = "password";
        private readonly HttpClient _httpClient = httpClient;

        public async Task<string> RegisterAsync(
            User user,
            string password,
            CancellationToken cancellationToken = default)
        {
            UserRepresentationModel userRepresentationModel = UserRepresentationModel.FromUser(user);

            userRepresentationModel.Credentials =
            [
                new()
                {
                    Value = password,
                    Temporary = false,
                    Type = PasswordCredentialType
                }
            ];

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                "users",
                userRepresentationModel,
                cancellationToken);

            return ExtractIdentityIdFromLocationHeader(response);
        }

        private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseMessage)
        {
            const string usersSegmentName = "users/";

            string? locationHeader = (httpResponseMessage.Headers.Location?.PathAndQuery)
                                     ?? throw new InvalidOperationException("Location header can't be null");

            int userSegmentValueIndex = locationHeader.IndexOf(usersSegmentName, StringComparison.InvariantCultureIgnoreCase);

            string userIdentityId = locationHeader[(userSegmentValueIndex + usersSegmentName.Length)..];

            return userIdentityId;
        }
    }
}

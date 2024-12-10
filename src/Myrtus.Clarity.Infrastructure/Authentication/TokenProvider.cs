using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions.Authentication;

namespace Myrtus.Clarity.Infrastructure.Authentication
{
    public class TokenProvider : IAccessTokenProvider
    {
        private readonly IConfidentialClientApplication _clientApplication;
        private readonly string[] _scopes;

        public TokenProvider(IConfidentialClientApplication clientApplication, string[] scopes)
        {
            _clientApplication = clientApplication;
            _scopes = scopes;
            AllowedHostsValidator = new AllowedHostsValidator();
        }

        public async Task<string> GetAuthorizationTokenAsync(
            Uri requestUri,
            Dictionary<string, object> additionalAuthenticationContext = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _clientApplication.AcquireTokenForClient(_scopes)
                    .ExecuteAsync(cancellationToken);

                return result.AccessToken;
            }
            catch (MsalServiceException ex)
            {
                // Log detailed error
                throw;
            }
        }

        public AllowedHostsValidator AllowedHostsValidator { get; }
    }
}
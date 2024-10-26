using System.Net.Http.Json;
using Myrtus.Clarity.Core.Infrastructure.Authentication.Keycloak.Models;
using Myrtus.CMS.Application.Abstractions.Auth;
using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Infrastructure.Authentication.Keycloak.Models;

namespace Myrtus.CMS.Infrastructure.Authentication.Keycloak;

public sealed class UserService : IUserService
{
    private const string PasswordCredentialType = "password";
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> RegisterAsync(
        User user,
        string password,
        CancellationToken cancellationToken = default)
    {
        var userRepresentationModel = UserRepresentationModel.FromUser(user);

        userRepresentationModel.Credentials = new CredentialRepresentationModel[]
        {
            new()
            {
                Value = password,
                Temporary = false,
                Type = PasswordCredentialType
            }
        };

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

        string userIdentityId = locationHeader.Substring(userSegmentValueIndex + usersSegmentName.Length);

        return userIdentityId;
    }
}

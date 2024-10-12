using Ardalis.Result;

namespace Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;

public interface IJwtService
{
    Task<Result<string>> GetAccessTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}

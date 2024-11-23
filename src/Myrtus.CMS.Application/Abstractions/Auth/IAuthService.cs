using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Abstractions.Auth;

public interface IAuthService
{
    Task<string> RegisterAsync(
        User user,
        string password,
        CancellationToken cancellationToken = default);
}

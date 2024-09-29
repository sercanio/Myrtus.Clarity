using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Abstractions.Authentication;

public interface IAuthenticationService
{
    Task<string> RegisterAsync(
        User user,
        string password,
        CancellationToken cancellationToken = default);
}

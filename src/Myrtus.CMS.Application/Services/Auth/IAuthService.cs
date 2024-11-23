using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(
            User user,
            string password,
            CancellationToken cancellationToken = default);
    }
}

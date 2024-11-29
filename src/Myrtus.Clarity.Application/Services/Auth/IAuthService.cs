using Myrtus.Clarity.Domain.Users;

namespace Myrtus.Clarity.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(
            User user,
            string password,
            CancellationToken cancellationToken = default);
    }
}

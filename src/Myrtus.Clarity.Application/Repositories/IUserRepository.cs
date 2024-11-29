using Myrtus.Clarity.Core.Application.Repositories;
using Myrtus.Clarity.Domain.Users;

namespace Myrtus.Clarity.Application.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

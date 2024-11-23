using Myrtus.Clarity.Core.Application.Repositories;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

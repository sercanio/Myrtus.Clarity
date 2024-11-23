using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.Clarity.Domain.Users;
using System.Linq.Expressions;

namespace Myrtus.Clarity.Application.Services.Users
{
    public interface IUserService
    {
        Task<User> GetAsync(
            Expression<Func<User, bool>> predicate,
            bool includeSoftDeleted = false,
            CancellationToken cancellationToken = default,
            params Expression<Func<User, object>>[] include);

        Task<User> GetUserByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<PaginatedList<User>> GetAllAsync(
            int index = 0,
            int size = 10,
            bool includeSoftDeleted = false,
            Expression<Func<User, bool>>? predicate = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<User, object>>[] include);

        Task AddAsync(User user);

        void Update(User user);

        void Delete(User user);
    }
}

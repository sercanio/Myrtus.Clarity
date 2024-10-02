using Myrtus.CMS.Domain.Blogs;

namespace Myrtus.CMS.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, params System.Linq.Expressions.Expression<Func<User, object>>[]? include);

    void Add(User user);
}

using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Domain.Blogs.Events;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetAsync(user => user.Id == id, cancellationToken: cancellationToken);
    }

    public override async Task AddAsync(User user)
    {
        foreach (Role role in user.Roles)
        {
            DbContext.Attach(role);
        }

      await DbContext.AddAsync(user);
    }
}

using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Application.Repositories;

namespace Myrtus.CMS.Infrastructure.Repositories
{
    internal sealed class UserRepository(ApplicationDbContext dbContext) : Repository<User>(dbContext), IUserRepository
    {
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
}

using Myrtus.Clarity.Core.Domain.Users;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}

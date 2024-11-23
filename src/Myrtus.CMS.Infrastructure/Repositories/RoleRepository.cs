using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Infrastructure.Repositories
{
    internal sealed class RoleRepository(ApplicationDbContext dbContext) : Repository<Role>(dbContext), IRoleRepository
    {
        public override async Task AddAsync(Role role)
        {
            await DbContext.AddAsync(role);
        }

        public override void Update(Role role)
        {
            DbContext.Update(role);
        }
    }
}

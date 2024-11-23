using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Domain.Roles;

namespace Myrtus.Clarity.Infrastructure.Repositories
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

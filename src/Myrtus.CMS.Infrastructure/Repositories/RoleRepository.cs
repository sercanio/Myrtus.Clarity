using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Roles.Events;

namespace Myrtus.CMS.Infrastructure.Repositories;

internal sealed class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public override async Task AddAsync(Role role)
    {
        await DbContext.AddAsync(role);
    }

    public override void Update(Role role)
    {
        DbContext.Update(role);
    }
}

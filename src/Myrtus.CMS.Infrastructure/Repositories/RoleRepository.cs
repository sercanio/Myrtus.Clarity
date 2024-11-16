using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;

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

    public override void Delete(Role role, bool isSoftDelete = true)
    {
        if (role.IsDefault)
        {
            throw new InvalidOperationException("Cannot delete default role.");
        }
        if (isSoftDelete)
        {
            role.MarkDeleted();
            DbContext.Update(role);
        }
        else
        {
            DbContext.Remove(role);
        }
    }
}

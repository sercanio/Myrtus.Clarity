using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Roles;
using System.Linq.Expressions;

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

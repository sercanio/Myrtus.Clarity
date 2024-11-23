using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Infrastructure.Repositories
{
    internal sealed class PermissionRepository(ApplicationDbContext dbContext) : Repository<Permission>(dbContext), IPermissionRepository
    {
    }
}

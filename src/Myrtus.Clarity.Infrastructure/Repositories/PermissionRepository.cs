using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Domain.Roles;

namespace Myrtus.Clarity.Infrastructure.Repositories
{
    internal sealed class PermissionRepository(ApplicationDbContext dbContext) : Repository<Permission>(dbContext), IPermissionRepository
    {
    }
}

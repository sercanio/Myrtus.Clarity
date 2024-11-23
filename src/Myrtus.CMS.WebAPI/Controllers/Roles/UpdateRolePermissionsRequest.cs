using Myrtus.CMS.Application.Enums;

namespace Myrtus.CMS.WebAPI.Controllers.Roles
{
    public sealed record UpdateRolePermissionsRequest(Guid PermissionId, Operation Operation);
}

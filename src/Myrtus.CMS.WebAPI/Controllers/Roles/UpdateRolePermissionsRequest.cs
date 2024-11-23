using Myrtus.Clarity.Application.Enums;

namespace Myrtus.Clarity.WebAPI.Controllers.Roles
{
    public sealed record UpdateRolePermissionsRequest(Guid PermissionId, Operation Operation);
}

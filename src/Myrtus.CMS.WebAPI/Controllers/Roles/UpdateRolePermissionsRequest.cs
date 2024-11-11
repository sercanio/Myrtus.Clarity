using Myrtus.CMS.Application.Enums;

namespace Myrtus.CMS.WebAPI.Controllers.UserRoles;

public sealed record UpdateRolePermissionsRequest(Guid PermissionId, OperationEnum Operation);

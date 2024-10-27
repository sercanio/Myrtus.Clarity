using Myrtus.CMS.Application.Users.Update.UpdateUserRoles;

namespace Myrtus.CMS.WebAPI.Controllers.Users;

public sealed record UpdateUserRolesRequest(OperationEnum Operation, Guid RoleId);
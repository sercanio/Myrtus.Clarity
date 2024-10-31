using Myrtus.CMS.Application.Users.Commands.Update.UpdateUserRoles;

namespace Myrtus.CMS.WebAPI.Controllers.Users;

public sealed record UpdateUserRolesRequest(OperationEnum Operation, Guid RoleId);
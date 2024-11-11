using Myrtus.CMS.Application.Enums;

namespace Myrtus.CMS.WebAPI.Controllers.Users;

public sealed record UpdateUserRolesRequest(OperationEnum Operation, Guid RoleId);
using Myrtus.CMS.Application.Enums;

namespace Myrtus.CMS.WebAPI.Controllers.Users;

public sealed record UpdateUserRolesRequest(Operation Operation, Guid RoleId);
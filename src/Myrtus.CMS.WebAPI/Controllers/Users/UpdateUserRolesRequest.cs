using Myrtus.Clarity.Application.Enums;

namespace Myrtus.Clarity.WebAPI.Controllers.Users;

public sealed record UpdateUserRolesRequest(Operation Operation, Guid RoleId);
namespace Myrtus.CMS.WebAPI.Controllers.Users;

public sealed record TakeRoleFromUserRequest(Guid RoleId, Guid UserId);
namespace Myrtus.CMS.WebAPI.Controllers.Users;

public sealed record GiveRoleToUserRequest(Guid RoleId, Guid UserId);
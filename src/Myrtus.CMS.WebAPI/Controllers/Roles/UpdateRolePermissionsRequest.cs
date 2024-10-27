using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.WebAPI.Controllers.UserRoles;

public sealed record UpdateRolePermissionsRequest(ICollection<Permission> permissions);

using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Application.Roles.Commands.Update;

public sealed record UpdateRoleCommandResponse(
    Guid RoleId, 
    ICollection<Permission> Permissions);
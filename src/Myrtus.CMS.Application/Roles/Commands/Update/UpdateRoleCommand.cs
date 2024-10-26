using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Application.Roles.Commands.Update;

public sealed record UpdateRoleCommand(
    Guid RoleId,
    ICollection<Permission> Permissions) : ICommand<UpdateRoleCommandResponse>;
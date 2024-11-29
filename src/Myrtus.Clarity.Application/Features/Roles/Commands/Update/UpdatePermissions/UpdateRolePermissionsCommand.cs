using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Application.Enums;

namespace Myrtus.Clarity.Application.Features.Roles.Commands.Update.UpdatePermissions
{
    public sealed record UpdateRolePermissionsCommand(
        Guid RoleId,
        Guid PermissionId,
        Operation Operation) : ICommand<UpdateRolePermissionsCommandResponse>;
}
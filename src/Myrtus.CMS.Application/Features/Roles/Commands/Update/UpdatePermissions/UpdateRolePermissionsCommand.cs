using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Application.Enums;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Update.UpdatePermissions
{
    public sealed record UpdateRolePermissionsCommand(
        Guid RoleId,
        Guid PermissionId,
        Operation Operation) : ICommand<UpdateRolePermissionsCommandResponse>;
}
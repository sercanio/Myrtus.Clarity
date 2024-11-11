using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Application.Enums;

namespace Myrtus.CMS.Application.Roles.Commands.Update;

public sealed record UpdateRolePermissionsCommand(
    Guid RoleId,
    Guid PermissionId,
    OperationEnum Operation) : ICommand<UpdateRolePermissionsCommandResponse>;
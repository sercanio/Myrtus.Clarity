using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.Clarity.Application.Features.Roles.Commands.Update.UpdateRoleName
{
    public sealed record UpdateRoleNameCommand(
        Guid RoleId,
        string Name) : ICommand<UpdateRoleNameCommandResponse>;
}
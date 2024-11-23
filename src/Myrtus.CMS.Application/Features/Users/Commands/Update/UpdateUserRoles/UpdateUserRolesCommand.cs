using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Application.Enums;

namespace Myrtus.Clarity.Application.Features.Users.Commands.Update.UpdateUserRoles
{
    public sealed record UpdateUserRolesCommand(
        Guid UserId,
        Operation Operation,
        Guid RoleId) : ICommand<UpdateUserRolesCommandResponse>;
}

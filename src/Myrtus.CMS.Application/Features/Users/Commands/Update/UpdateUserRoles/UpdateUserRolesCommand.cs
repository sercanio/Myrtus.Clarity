using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Application.Enums;

namespace Myrtus.CMS.Application.Features.Users.Commands.Update.UpdateUserRoles
{
    public sealed record UpdateUserRolesCommand(
        Guid UserId,
        Operation Operation,
        Guid RoleId) : ICommand<UpdateUserRolesCommandResponse>;
}

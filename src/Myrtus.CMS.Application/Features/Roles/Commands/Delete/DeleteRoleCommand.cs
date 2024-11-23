using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.Clarity.Application.Features.Roles.Commands.Delete
{
    public sealed record DeleteRoleCommand(Guid RoleId) : ICommand<DeleteRoleCommandResponse>;
}
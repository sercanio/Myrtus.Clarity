using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.Clarity.Application.Features.Roles.Commands.Create
{
    public sealed record CreateRoleCommand(string Name) : ICommand<CreateRoleCommandResponse>;
}

using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Create
{
    public sealed record CreateRoleCommand(string Name) : ICommand<CreateRoleCommandResponse>;
}

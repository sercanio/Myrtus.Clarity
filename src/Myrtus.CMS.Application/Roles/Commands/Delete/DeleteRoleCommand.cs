using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Roles.Commands.Delete;

public sealed record DeleteRoleCommand(Guid RoleId): ICommand<DeleteRoleCommandResponse>;
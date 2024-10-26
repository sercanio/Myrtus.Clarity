using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Users.GiveRoleToUser;

public sealed record GiveRoleToUserCommand(Guid RoleId, Guid UserId) : ICommand<GiveRoleToUserCommandResponse>;
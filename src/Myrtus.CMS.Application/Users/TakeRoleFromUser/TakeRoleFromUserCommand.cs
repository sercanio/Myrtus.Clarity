using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using System.Windows.Input;

namespace Myrtus.CMS.Application.Users.TakeRoleFromUser;

public sealed record TakeRoleFromUserCommand(Guid RoleId, Guid UserId) : ICommand<TakeRoleFromUserCommandResponse>;

using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Users.Update.UpdateUserRoles;

public sealed record UpdateUserRolesCommand(
    Guid UserId,
    OperationEnum Operation, 
    Guid RoleId) : ICommand<UpdateUserRolesCommandResponse>;

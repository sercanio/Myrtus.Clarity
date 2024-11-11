﻿using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Application.Enums;

namespace Myrtus.CMS.Application.Users.Commands.Update.UpdateUserRoles;

public sealed record UpdateUserRolesCommand(
    Guid UserId,
    OperationEnum Operation,
    Guid RoleId) : ICommand<UpdateUserRolesCommandResponse>;

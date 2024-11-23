﻿using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Features.Accounts.RegisterUser
{
    public sealed record RegisterUserCommand(
            string Email,
            string FirstName,
            string LastName,
            string Password) : ICommand<Guid>;
}

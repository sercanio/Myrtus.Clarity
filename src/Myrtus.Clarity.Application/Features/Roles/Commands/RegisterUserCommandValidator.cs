﻿using FluentValidation;

namespace Myrtus.Clarity.Application.Features.Accounts.RegisterUser
{
    internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty();

            RuleFor(c => c.LastName).NotEmpty();

            RuleFor(c => c.Email.Value).EmailAddress();

            RuleFor(c => c.Password).NotEmpty().MinimumLength(5);
        }
    }
}

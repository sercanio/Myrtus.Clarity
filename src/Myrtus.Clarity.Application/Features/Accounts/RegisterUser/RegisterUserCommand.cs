using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Domain.Users.ValueObjects;

namespace Myrtus.Clarity.Application.Features.Accounts.RegisterUser
{
    public sealed record RegisterUserCommand(
            Email Email,
            FirstName FirstName,
            LastName LastName,
            string Password) : ICommand<Guid>;
}

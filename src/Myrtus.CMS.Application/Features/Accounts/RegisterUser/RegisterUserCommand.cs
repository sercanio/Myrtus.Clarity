using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.Clarity.Application.Features.Accounts.RegisterUser
{
    public sealed record RegisterUserCommand(
            string Email,
            string FirstName,
            string LastName,
            string Password) : ICommand<Guid>;
}

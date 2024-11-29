using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.Clarity.Application.Features.Accounts.LogInUser
{
    public sealed record LogInUserCommand(string Email, string Password)
        : ICommand<AccessTokenResponse>;
}

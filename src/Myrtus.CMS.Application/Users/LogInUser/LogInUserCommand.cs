using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Users.LogInUser;

public sealed record LogInUserCommand(string Email, string Password)
    : ICommand<AccessTokenResponse>;

using Myrtus.Clarity.Domain.Users.ValueObjects;

namespace Myrtus.Clarity.WebAPI.Controllers.Accounts
{
    public sealed record RegisterUserRequest(
        Email Email,
        FirstName FirstName,
        LastName LastName,
        string Password);
}

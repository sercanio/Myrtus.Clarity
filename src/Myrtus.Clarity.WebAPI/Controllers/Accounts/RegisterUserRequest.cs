using Myrtus.Clarity.Domain.Users.ValueObjects;

namespace Myrtus.Clarity.WebAPI.Controllers.Accounts
{
    public sealed record RegisterUserRequest(
        string Email,
        string FirstName,
        string LastName,
        string Password);
}

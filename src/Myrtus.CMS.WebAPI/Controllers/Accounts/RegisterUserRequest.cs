namespace Myrtus.CMS.WebAPI.Controllers.Accounts;

public sealed record RegisterUserRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password);

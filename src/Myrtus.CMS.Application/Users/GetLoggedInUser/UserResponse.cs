namespace Myrtus.CMS.Application.Users.GetLoggedInUser;

public sealed record UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    private UserResponse() { }
};
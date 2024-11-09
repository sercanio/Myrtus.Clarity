namespace Myrtus.CMS.Application.Users.GetLoggedInUser;

public sealed record LoggedInUserRolesDto
{
    public string Name { get; set; }

    public LoggedInUserRolesDto(string name)
    {
        Name = name;
    }
};
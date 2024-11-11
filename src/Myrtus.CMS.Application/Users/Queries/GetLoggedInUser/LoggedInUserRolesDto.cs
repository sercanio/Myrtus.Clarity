namespace Myrtus.CMS.Application.Users.GetLoggedInUser;

public sealed record LoggedInUserRolesDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public LoggedInUserRolesDto(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
};
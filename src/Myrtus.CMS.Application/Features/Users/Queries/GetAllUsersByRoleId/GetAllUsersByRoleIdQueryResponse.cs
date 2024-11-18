using Myrtus.CMS.Application.Features.Users.Queries.GetLoggedInUser;

namespace Myrtus.CMS.Application.Features.Users.Queries.GetAllUsersByRoleId;

public sealed record GetAllUsersByRoleIdQueryResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<LoggedInUserRolesDto> Roles { get; set; } = [];

    public GetAllUsersByRoleIdQueryResponse(Guid id, string email, string? firstName, string? lastName, List<LoggedInUserRolesDto> roles)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Roles = roles;
    }

    public GetAllUsersByRoleIdQueryResponse(Guid id, string email, string? firstName, string? lastName)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public GetAllUsersByRoleIdQueryResponse() { }
}

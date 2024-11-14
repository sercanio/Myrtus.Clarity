using Myrtus.CMS.Application.Users.GetLoggedInUser;

namespace Myrtus.CMS.Application.Users.Queries.GetAllUsersDynamic;

public sealed record GetAllUsersDynamicQueryResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public List<LoggedInUserRolesDto> Roles { get; init; } = new();

    public GetAllUsersDynamicQueryResponse(Guid id, string email, string? firstName, string? lastName, List<LoggedInUserRolesDto> roles)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Roles = roles;
    }
}
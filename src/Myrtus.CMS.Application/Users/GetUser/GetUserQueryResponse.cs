using Myrtus.CMS.Application.Roles.Queries.GetRoleById;

namespace Myrtus.CMS.Application.Users.GetUser;

public sealed record GetUserQueryResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ICollection<GetRoleByIdQueryResponse> Roles { get; set; } = [];


    public GetUserQueryResponse(string email, string? firstName, string? lastName, ICollection<GetRoleByIdQueryResponse> roles)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Roles = roles;
    }
    public GetUserQueryResponse() { }
}

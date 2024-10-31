using Myrtus.CMS.Application.Roles.Queries.GetRoleById;

namespace Myrtus.CMS.Application.Users.Queries.GetUser;

public sealed record GetUserQueryResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ICollection<GetRoleByIdQueryResponse> Roles { get; set; } = [];


    public GetUserQueryResponse(Guid id, string email, string? firstName, string? lastName, ICollection<GetRoleByIdQueryResponse> roles)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Roles = roles;
    }
    
    public GetUserQueryResponse(Guid id, string email, string? firstName, string? lastName)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }


    public GetUserQueryResponse() { }
}

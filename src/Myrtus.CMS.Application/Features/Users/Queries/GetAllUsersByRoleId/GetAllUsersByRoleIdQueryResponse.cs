using Myrtus.Clarity.Application.Features.Users.Queries.GetLoggedInUser;
using System.Collections.ObjectModel;

namespace Myrtus.Clarity.Application.Features.Users.Queries.GetAllUsersByRoleId
{
    public sealed record GetAllUsersByRoleIdQueryResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public ICollection<LoggedInUserRolesDto> Roles { get; set; } = [];

        public GetAllUsersByRoleIdQueryResponse(
            Guid id,
            string email,
            string? firstName,
            string? lastName,
            Collection<LoggedInUserRolesDto> roles)
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
}

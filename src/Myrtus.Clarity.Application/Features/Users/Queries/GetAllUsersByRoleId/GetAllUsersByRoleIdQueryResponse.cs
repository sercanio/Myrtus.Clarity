using Myrtus.Clarity.Application.Features.Users.Queries.GetLoggedInUser;
using Myrtus.Clarity.Domain.Users.ValueObjects;
using System.Collections.ObjectModel;

namespace Myrtus.Clarity.Application.Features.Users.Queries.GetAllUsersByRoleId
{
    public sealed record GetAllUsersByRoleIdQueryResponse
    {
        public Guid Id { get; set; }
        public Email Email { get; set; } = new Email(string.Empty);
        public FirstName? FirstName { get; set; }
        public LastName? LastName { get; set; }
        public ICollection<LoggedInUserRolesDto> Roles { get; set; } = [];

        public GetAllUsersByRoleIdQueryResponse(
            Guid id,
            Email email,
            FirstName? firstName,
            LastName? lastName,
            Collection<LoggedInUserRolesDto> roles)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Roles = roles;
        }

        public GetAllUsersByRoleIdQueryResponse(
            Guid id, Email email, FirstName? firstName, LastName? lastName)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public GetAllUsersByRoleIdQueryResponse() { }
    }
}

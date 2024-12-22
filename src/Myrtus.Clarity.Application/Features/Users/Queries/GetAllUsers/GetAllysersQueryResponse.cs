using Myrtus.Clarity.Application.Features.Users.Queries.GetLoggedInUser;
using Myrtus.Clarity.Domain.Users.ValueObjects;
using System.Collections.ObjectModel;

namespace Myrtus.Clarity.Application.Features.Users.Queries.GetAllUsers
{
    public sealed record GetAllUsersQueryResponse
    {
        public Guid Id { get; set; }
        public Email Email { get; set; }
        public FirstName? FirstName { get; set; }
        public LastName? LastName { get; set; }
        public ICollection<LoggedInUserRolesDto> Roles { get; set; } = [];

        public GetAllUsersQueryResponse(
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

        public GetAllUsersQueryResponse(
            Guid id,
            Email email,
            FirstName? firstName,
            LastName? lastName)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public GetAllUsersQueryResponse()
        {
        }
    }
}

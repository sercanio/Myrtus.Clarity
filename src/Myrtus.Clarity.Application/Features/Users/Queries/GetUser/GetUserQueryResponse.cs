using System.Collections.ObjectModel;
using Myrtus.Clarity.Application.Features.Roles.Queries.GetRoleById;
using Myrtus.Clarity.Domain.Users.ValueObjects;

namespace Myrtus.Clarity.Application.Features.Users.Queries.GetUser
{
    public sealed record GetUserQueryResponse
    {
        public Guid Id { get; set; }
        public Email Email { get; set; }
        public FirstName? FirstName { get; set; }
        public LastName? LastName { get; set; }
        public ICollection<GetRoleByIdQueryResponse> Roles { get; set; } = [];

        public GetUserQueryResponse(Guid id,
            Email email,
            FirstName? firstName,
            LastName? lastName,
            Collection<GetRoleByIdQueryResponse> roles)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Roles = roles;
        }

        public GetUserQueryResponse(Guid id, Email email, FirstName? firstName, LastName? lastName)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public GetUserQueryResponse() { }
    }
}

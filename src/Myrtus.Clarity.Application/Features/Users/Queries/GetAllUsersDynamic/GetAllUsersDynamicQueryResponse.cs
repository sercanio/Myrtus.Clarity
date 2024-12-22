using Myrtus.Clarity.Application.Features.Users.Queries.GetLoggedInUser;
using Myrtus.Clarity.Domain.Users.ValueObjects;
using System.Collections.ObjectModel;

namespace Myrtus.Clarity.Application.Features.Users.Queries.GetAllUsersDynamic
{
    public sealed record GetAllUsersDynamicQueryResponse
    {
        public Guid Id { get; init; }
        public Email Email { get; init; }
        public FirstName? FirstName { get; init; }
        public LastName? LastName { get; init; }
        public ICollection<LoggedInUserRolesDto> Roles { get; init; } = [];

        public GetAllUsersDynamicQueryResponse(
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
    }
}
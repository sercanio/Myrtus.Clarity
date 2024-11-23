using Myrtus.Clarity.Application.Features.Users.Queries.GetLoggedInUser;
using System.Collections.ObjectModel;

namespace Myrtus.Clarity.Application.Features.Users.Queries.GetAllUsersDynamic
{
    public sealed record GetAllUsersDynamicQueryResponse
    {
        public Guid Id { get; init; }
        public string Email { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public ICollection<LoggedInUserRolesDto> Roles { get; init; } = [];

        public GetAllUsersDynamicQueryResponse(
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
    }
}
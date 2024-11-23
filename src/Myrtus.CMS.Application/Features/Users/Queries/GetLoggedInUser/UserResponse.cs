using System.Collections.ObjectModel;

namespace Myrtus.CMS.Application.Features.Users.Queries.GetLoggedInUser
{
    public sealed record UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<LoggedInUserRolesDto> Roles { get; set; } = [];

        public UserResponse(
            Guid id,
            string email,
            string firstName,
            string lastName,
            Collection<LoggedInUserRolesDto> roles)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Roles = roles;
        }

        internal UserResponse() { }
    }
}

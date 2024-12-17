using Myrtus.Clarity.Domain.Users.ValueObjects;
using System.Collections.ObjectModel;

namespace Myrtus.Clarity.Application.Features.Users.Queries.GetLoggedInUser
{
    public sealed record UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<LoggedInUserRolesDto> Roles { get; set; } = [];
        public NotificationPreference NotificationPreference { get; set; }

        public UserResponse(
            Guid id,
            string email,
            string firstName,
            string lastName,
            Collection<LoggedInUserRolesDto> roles,
            NotificationPreference notificationPreference)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Roles = roles;
            NotificationPreference = notificationPreference;
        }

        internal UserResponse() { }
    }
}

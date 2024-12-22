using Myrtus.Clarity.Domain.Users.ValueObjects;
using System.Collections.ObjectModel;

namespace Myrtus.Clarity.Application.Features.Users.Queries.GetLoggedInUser
{
    public sealed record UserResponse
    {
        public Guid Id { get; set; }
        public Email Email { get; set; }
        public FirstName FirstName { get; set; }
        public LastName LastName { get; set; }
        public ICollection<LoggedInUserRolesDto> Roles { get; set; } = [];
        public NotificationPreference NotificationPreference { get; set; }

        public UserResponse(
            Guid id,
            Email email,
            FirstName firstName,
            LastName lastName,
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

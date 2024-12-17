using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Users.Events;
using Myrtus.Clarity.Domain.Users.ValueObjects;

namespace Myrtus.Clarity.Domain.Users
{
    public sealed class User : Entity, IAggregateRoot
    {
        private readonly List<Role> _roles = [];
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string IdentityId { get; private set; } = string.Empty;
        public NotificationPreference NotificationPreference { get; private set; }

        public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

        private User(
            Guid id,
            string firstName,
            string lastName,
            string email,
            NotificationPreference notificationPreference) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            NotificationPreference = notificationPreference;
        }

        private User()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
        }

        public static User Create(string firstName,
            string lastName,
            string email)
        {
            NotificationPreference notificationPreference = new(true, true, true);
            User user = new(Guid.NewGuid(), firstName, lastName, email, notificationPreference);
            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
            user.AddRole(Role.DefaultRole);
            user.UpdatedBy = "System";
            return user;
        }

        public static User CreateWithoutRolesForSeeding(
            string firstName,
            string lastName,
            string email)
        {
            NotificationPreference notificationPreference = new(true, true, true);
            return new User(Guid.NewGuid(), firstName, lastName, email, notificationPreference);
        }

        public void AddRole(Role role)
        {
            this._roles.Add(role);
            this.RaiseDomainEvent(new UserRoleAddedDomainEvent(this.Id, role.Id));
        }

        public void RemoveRole(Role role)
        {
            _ = this._roles.Remove(role);
            this.RaiseDomainEvent(new UserRoleRemovedDomainEvent(this.Id, role.Id));
        }

        public void SetIdentityId(string identityId)
        {
            IdentityId = identityId;
        }
    }
}

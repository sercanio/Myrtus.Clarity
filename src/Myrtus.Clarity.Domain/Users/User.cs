using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Users.Events;
using Myrtus.Clarity.Domain.Users.ValueObjects;

namespace Myrtus.Clarity.Domain.Users
{
    public sealed class User : Entity, IAggregateRoot
    {
        private readonly List<Role> _roles = new();
        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public Email Email { get; private set; }
        public string IdentityId { get; private set; } = string.Empty;
        public NotificationPreference NotificationPreference { get; private set; }

        public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

        private User(
            Guid id,
            FirstName firstName,
            LastName lastName,
            Email email,
            NotificationPreference notificationPreference) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            NotificationPreference = notificationPreference;
        }

        private User()
        {
            FirstName = new FirstName("DefaultFirstName");
            LastName = new LastName("DefaultLastName");
            Email = new Email("default@example.com");
        }

        public static User Create(FirstName firstName,
            LastName lastName,
            Email email)
        {
            NotificationPreference notificationPreference = new(true, true, true);
            User user = new(Guid.NewGuid(), firstName, lastName, email, notificationPreference);
            user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
            user.AddRole(Role.DefaultRole);
            user.UpdatedBy = "System";
            return user;
        }

        public static User CreateWithoutRolesForSeeding(
            FirstName firstName,
            LastName lastName,
            Email email)
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

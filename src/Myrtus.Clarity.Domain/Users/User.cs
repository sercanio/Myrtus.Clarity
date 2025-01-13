using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Users.Events;
using Myrtus.Clarity.Domain.Users.ValueObjects;
using System.Collections.Generic;

namespace Myrtus.Clarity.Domain.Users
{
    public sealed class User : Entity, IAggregateRoot
    {
        private readonly List<Role> _roles = new();
        public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public Email Email { get; private set; }
        public string IdentityId { get; private set; } = string.Empty;
        public NotificationPreference NotificationPreference { get; private set; }

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
        }

        public static User Create(
            FirstName firstName,
            LastName lastName,
            Email email)
        {
            NotificationPreference notificationPreference = new(true, true, true);
            var user = new User(Guid.NewGuid(), firstName, lastName, email, notificationPreference);
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
            return new User(Guid.Parse("55c7f429-0916-4d84-8b76-d45185d89aa7"), firstName, lastName, email, notificationPreference);
        }

        public void AddRole(Role role)
        {
            if (!_roles.Contains(role))
            {
                _roles.Add(role);
                RaiseDomainEvent(new UserRoleAddedDomainEvent(Id, role.Id));
            }
        }

        public void RemoveRole(Role role)
        {
            if (_roles.Remove(role))
            {
                RaiseDomainEvent(new UserRoleRemovedDomainEvent(Id, role.Id));
            }
        }

        public void SetIdentityId(string identityId)
        {
            IdentityId = identityId;
        }
    }
}

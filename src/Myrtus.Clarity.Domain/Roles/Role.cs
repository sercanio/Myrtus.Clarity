﻿using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Domain.Roles.Events;
using Myrtus.Clarity.Domain.Users;
using System.Collections.Generic;

namespace Myrtus.Clarity.Domain.Roles
{
    public sealed class Role : Entity, IAggregateRoot
    {
        public static readonly Role DefaultRole = Create(
            Guid.Parse("5dc6ec47-5b7c-4c2b-86cd-3a671834e56e"),
            "Registered",
            isDefault: true);

        public static readonly Role Admin = Create(
            Guid.Parse("4b606d86-3537-475a-aa20-26aadd8f5cfd"),
            "Admin",
            isDefault: false);

        private readonly List<User> _users = new();
        public IReadOnlyCollection<User> Users => _users.AsReadOnly();

        private readonly List<Permission> _permissions = new();
        public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

        public string Name { get; private set; }
        public bool IsDefault { get; private set; }

        public Role(Guid id, string name, bool isDefault = false) : base(id)
        {
            Name = name;
            IsDefault = isDefault;
        }

        public Role() : base(Guid.NewGuid())
        {
            Name = string.Empty;
        }

        // Used for data seeding for default roles
        private static Role Create(Guid Id, string Name, bool isDefault = false)
        {
            return new Role(Id, Name, isDefault);
        }

        public static Role Create(string Name, bool isDefault = false)
        {
            var role = new Role(Guid.NewGuid(), Name, isDefault);
            role.RaiseDomainEvent(new RoleCreatedDomainEvent(role.Id));
            return role;
        }

        public static Role Delete(Role role)
        {
            role.RaiseDomainEvent(new RoleDeletedDomainEvent(role.Id));
            role.MarkDeleted();
            return role;
        }

        public void ChangeName(string newName)
        {
            var oldName = this.Name;
            this.Name = newName;
            this.RaiseDomainEvent(new RoleNameUpdatedDomainEvent(this.Id, oldName));
        }

        // Domain methods for Permissions
        public void AddPermission(Permission permission)
        {
            if (!_permissions.Contains(permission))
            {
                _permissions.Add(permission);
                RaiseDomainEvent(new RolePermissionAddedDomainEvent(this.Id, permission.Id));
            }
        }

        public void RemovePermission(Permission permission)
        {
            if (_permissions.Remove(permission))
            {
                RaiseDomainEvent(new RolePermissionRemovedDomainEvent(this.Id, permission.Id));
            }
        }

        // Similarly, if you want domain methods for Users:
        public void AddUser(User user)
        {
            if (!_users.Contains(user))
            {
                _users.Add(user);
                // Possibly raise a domain event
            }
        }

        public void RemoveUser(User user)
        {
            _users.Remove(user);
            // Possibly raise a domain event
        }
    }
}

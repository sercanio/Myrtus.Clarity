using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Domain.Roles.Events;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Domain.Roles
{
    public sealed class Role : Entity, IAggregateRoot
    {
        public static readonly Role DefaultRole = Create(
            Guid.Parse("5dc6ec47-5b7c-4c2b-86cd-3a671834e56e"),
            "Registered",
            isDefault: true);
        public static readonly Role Admin = Create(Guid.Parse("4b606d86-3537-475a-aa20-26aadd8f5cfd"), "Admin", isDefault: false);

        private readonly List<User> _users = [];
        private readonly List<Permission> _permissions = [];
        public string Name { get; set; }
        public bool IsDefault { get; private set; }

        public IReadOnlyCollection<User> Users => _users.AsReadOnly();
        public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

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
            Role role = new(Id, Name, isDefault);
            return role;
        }

        public static Role Create(string Name, bool isDefault = false)
        {
            Role role = new(Guid.NewGuid(), Name, isDefault);
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
            string oldName = this.Name;
            this.Name = newName;
            this.RaiseDomainEvent(new RoleNameUpdatedDomainEvent(this.Id, oldName));
        }

        public void AddPermission(Permission permission)
        {
            this._permissions.Add(permission);
            this.RaiseDomainEvent(new RolePermissionAddedDomainEvent(this.Id, permission.Id));
        }

        public void RemovePermission(Permission permission)
        {
            _ = this._permissions.Remove(permission);
            this.RaiseDomainEvent(new RolePermissionRemovedDomainEvent(this.Id, permission.Id));
        }
    }
}

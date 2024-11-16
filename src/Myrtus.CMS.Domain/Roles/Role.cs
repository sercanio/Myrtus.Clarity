using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Domain.Roles;

public sealed class Role : Entity, IAggregateRoot
{
    public static readonly Role DefaultRole = Create(
        Guid.Parse("5dc6ec47-5b7c-4c2b-86cd-3a671834e56e"),
        "Registered",
        isDefault: true);
    public static readonly Role Admin = Create(Guid.Parse("4b606d86-3537-475a-aa20-26aadd8f5cfd"), "Admin", isDefault: false);

    public string Name { get; set; }
    public bool IsDefault { get; private set; }
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public Role(Guid id, string name, bool isDefault = false) : base(id)
    {
        Name = name;
        IsDefault = isDefault;
    }

    // Parameterless constructor for EF and JSON deserialization
    public Role() : base(Guid.NewGuid())
    {
    }

    private static Role Create(Guid Id, string Name, bool isDefault = false)
    {
        Role role = new(Id, Name, isDefault);
        return role;
    }

    public static Role Create(string Name, bool isDefault = false)
    {
        Role role = new(Guid.NewGuid(), Name, isDefault);
        return role;
    }

    public static Role ChangeName(Role role, string name)
    {
        role.Name = name;
        return role;
    }

    public static Role AddPermission(Role role, Permission permission)
    {
        role.Permissions.Add(permission);
        return role;
    }

    public static Role RemovePermission(Role role, Permission permission)
    {
        _ = role.Permissions.Remove(permission);
        return role;
    }
}

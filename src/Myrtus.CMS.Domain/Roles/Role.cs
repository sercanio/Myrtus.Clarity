using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Domain.Roles;

public sealed class Role : Entity
{
    public static readonly Role Registered = new(Guid.Parse("5dc6ec47-5b7c-4c2b-86cd-3a671834e56e"), "Registered");
    public static readonly Role Admin = new(Guid.Parse("4b606d86-3537-475a-aa20-26aadd8f5cfd"), "Admin");

    public Role(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; init; }

    public ICollection<User> Users { get; init; } = new List<User>();

    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public static Role Create(
        string Name)
    {
        Role role = new Role(
            Guid.NewGuid(),
            Name);

        return role;
    }

}

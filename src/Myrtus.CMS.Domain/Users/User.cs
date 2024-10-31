using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Users.Events;

namespace Myrtus.CMS.Domain.Users;

public sealed class User : Entity
{
    private readonly List<Role> _roles = new();
    private readonly List<Blog> _blogs = new();

    private User(Guid id, FirstName firstName, LastName lastName, Email email)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    private User()
    {
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public string IdentityId { get; private set; } = string.Empty;

    public ICollection<Role> Roles { get; set; }
    public ICollection<Blog> Blogs { get; set; }

    public static User Create(FirstName firstName, LastName lastName, Email email)
    {
        var user = new User(Guid.NewGuid(), firstName, lastName, email);
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        user._roles.Add(Role.Registered);
        return user;
    }

    public static User CreateWithoutRolesForSeeding(FirstName firstName, LastName lastName, Email email)
    {
        return new User(Guid.NewGuid(), firstName, lastName, email);
    }

    public static User AddRole(User user, Role role)
    {
        user._roles.Add(role);
        return user;
    }

    public static User RemoveRole(User user, Role role)
    {
        user._roles.Remove(role);
        return user;
    }

    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }
}

using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Users.Events;

namespace Myrtus.CMS.Domain.Users;

public sealed class User : Entity, IAggregateRoot
{
    private readonly List<Role> _roles = new();
    private readonly List<Blog> _blogs = new();

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string IdentityId { get; private set; } = string.Empty;

    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();
    public IReadOnlyCollection<Blog> Blogs => _blogs.AsReadOnly();

    private User(Guid id, string firstName, string lastName, string email)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    private User()
    {
    }

    public static User Create(string firstName, string lastName, string email)
    {
        User user = new(Guid.NewGuid(), firstName, lastName, email);
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        user.AddRole(Role.DefaultRole);
        user.UpdatedBy = "System";
        return user;
    }

    public static User CreateWithoutRolesForSeeding(string firstName, string lastName, string email)
    {
        return new User(Guid.NewGuid(), firstName, lastName, email);
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

    public void AddBlog(Blog blog)
    {
        if (_blogs.Contains(blog))
        {
            throw new InvalidOperationException("User already has this blog.");
        }
        _blogs.Add(blog);
    }

    public void RemoveBlog(Blog blog)
    {
        if (!_blogs.Contains(blog))
        {
            throw new InvalidOperationException("User does not have this blog.");
        }
        _ = _blogs.Remove(blog);
    }

    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }
}

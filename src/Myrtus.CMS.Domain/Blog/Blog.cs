using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Domain.Blogs.Posts;
using Myrtus.CMS.Domain.Blogs.Common;
using Myrtus.CMS.Domain.Blogs.Posts.Events;
using Myrtus.CMS.Domain.Users.Events;
using Myrtus.CMS.Domain.Common;

namespace Myrtus.CMS.Domain.Blogs;

public sealed class Blog : Entity
{
    private readonly List<Post> _posts = new();

    private Blog(
        Guid id,
        Title title,
        Slug slug,
        User owner)
        : base(id)
    {
        Title = title;
        Slug = slug;
        Owner = owner;
    }

    private Blog()
    {
    }

    public Title Title { get; private set; }
    public Slug Slug { get; private set; }
    public User Owner { get; private set; }
    public User? LastUpdatedBy { get; private set; }
    public Description? UpdateReason { get; private set; }
    public User? DeletedBy { get; private set; }
    public Description? DeleteReason { get; private set; }

    public IReadOnlyCollection<Post> Posts => _posts.AsReadOnly();

    public static Blog Create(
        Title title, 
        Slug slug, 
        User owner)
    {
        Blog blog = new Blog(
            Guid.NewGuid(),
            title,
            slug,
            owner);

        return blog;
    }

    public void Delete()
    {
        foreach (var post in _posts)
        {
            RaiseDomainEvent(new PostRemovedEvent(post));
        }

        _posts.Clear();
        MarkDeleted();
    }

    public void ChangeTitle(Title title)
    {
        Title = title;
        MarkUpdated();
    }

    public void ChangeSlug(Slug slug)
    {
        Slug = slug;
        MarkUpdated();
    }

    public void AddPost(Post post)
    {
        _posts.Add(post);
        MarkUpdated();
    }

    public void RemovePost(Post post)
    {
        _posts.Remove(post);
        MarkUpdated();
        RaiseDomainEvent(new PostRemovedEvent(post));
    }
}

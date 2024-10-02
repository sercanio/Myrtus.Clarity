using Myrtus.Clarity.Core.Application.Abstractions.Clock;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Domain.Blogs.Common;
using Myrtus.CMS.Domain.Blogs.Events;
using Myrtus.CMS.Domain.Blogs.Posts.Events;

namespace Myrtus.CMS.Domain.Blogs.Posts;

public sealed class Post : Entity
{
    private readonly List<Comment> _comments = new();

    private Post(
        Guid id,
        Blog blog,
        Title title,
        Slug slug,
        Summary summary,
        Content content,
        CoverImageUrl coverImageUrl,
        CardImageUrl cardImageUrl,
        PostStatus status,
        Reviewed reviewed)
        : base(id)
    {
        Title = title;
        Slug = slug;
        Summary = summary;
        Content = content;
        Blog = blog;
        CoverImage = coverImageUrl;
        CardImage = cardImageUrl;
        Status = status;
        Reviewed = reviewed;
    }

    private Post()
    {
    }

    public Title Title { get; private set; }
    public Slug Slug { get; private set; }
    public Summary Summary { get; private set; }
    public Content Content { get; private set; }
    public Blog Blog { get; private set; }
    public CoverImageUrl CoverImage { get; private set; }
    public CardImageUrl CardImage { get; private set; }
    public PostStatus Status { get; private set; }
    public Reviewed Reviewed { get; private set; }

    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    public static Post Create(
        Blog blog,
        Title title,
        Slug slug,
        Summary summary,
        Content content,
        CoverImageUrl coverImage,
        CardImageUrl cardImage)
    {
        return new Post(
            Guid.NewGuid(),
            blog,
            title,
            slug,
            summary,
            content,
            coverImage,
            cardImage,
            PostStatus.Draft,
            new Reviewed(false));
    }

    public void DeletePost(Guid id)
    {
        RaiseDomainEvent(new PostRemovedEvent(this));
    }

    public void ChangeTitle(Title title)
    {
        Title = title;
    }

    public void ChangeSlug(Slug slug)
    {
        Slug = slug;
    }

    public void ChangeSummary(Summary summary)
    {
        Summary = summary;
    }

    public void ChangeContent(Content content)
    {
        Content = content;
    }

    public void ChangeCoverImage(CoverImageUrl coverImage)
    {
        CoverImage = coverImage;
    }

    public void ChangeCardImage(CardImageUrl cardImage)
    {
        CardImage = cardImage;
    }

    public void ChangeStatus(PostStatus newStatus)
    {
        Status = newStatus;
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public void RemoveComment(Comment comment)
    {
        _comments.Remove(comment);
        RaiseDomainEvent(new CommentRemovedEvent(this, comment));
    }
}
using Myrtus.Clarity.Core.Application.Abstractions.Clock;
using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Blogs.Posts;

public sealed class Comment : Entity
{
    private Comment(Guid id, Content content, Post post)
        : base(id)
    {
        Content = content;
        Post = post;
    }

    private Comment()
    {
    }

    public Content Content { get; private set; }
    public Post Post { get; private set; }

    public static Comment Create(Content content, Post post)
    {
        return new Comment(Guid.NewGuid(), content, post);
    }
}

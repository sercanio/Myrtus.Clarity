using MediatR;

namespace Myrtus.CMS.Application.Blogs.Commands.DeleteBlog;

public class DeleteBlogCommand : IRequest<bool>
{
    public Guid BlogId { get; }

    public DeleteBlogCommand(Guid blogId)
    {
        BlogId = blogId;
    }
}

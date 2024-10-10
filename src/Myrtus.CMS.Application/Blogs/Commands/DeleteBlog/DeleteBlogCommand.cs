using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Application.Blogs.Commands.DeleteBlog;

public class DeleteBlogCommand : ICommand<DeleteBlogCommandResponse>
{
    public Guid BlogId { get; }

    public DeleteBlogCommand(Guid blogId)
    {
        BlogId = blogId;
    }
}

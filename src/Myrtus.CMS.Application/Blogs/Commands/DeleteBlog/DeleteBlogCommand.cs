using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Blogs.Commands.DeleteBlog;

public record DeleteBlogCommand(Guid BlogId) : ICommand<DeleteBlogCommandResponse>;

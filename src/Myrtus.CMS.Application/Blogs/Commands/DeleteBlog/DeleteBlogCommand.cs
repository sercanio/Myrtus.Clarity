using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Ardalis.Result;

namespace Myrtus.CMS.Application.Blogs.Commands.DeleteBlog;

public record DeleteBlogCommand(Guid BlogId) : ICommand<DeleteBlogCommandResponse>;

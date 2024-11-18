using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Features.Blogs.Commands.Delete;

public record DeleteBlogCommand(Guid BlogId) : ICommand<DeleteBlogCommandResponse>;

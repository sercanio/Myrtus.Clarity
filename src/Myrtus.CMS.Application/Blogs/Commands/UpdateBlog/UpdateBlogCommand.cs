using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Blogs.Commands.UpdateBlog;

public sealed record UpdateBlogCommand(
    Guid BlogId,
    Guid UpdatedById,
    string? Title,
    string? Slug,
    string? Description) : ICommand<UpdateBlogCommandResponse>;
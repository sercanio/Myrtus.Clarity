using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Blogs.Commands.UpdateBlog;

public sealed record UpdateBlogCommand(
    Guid BlogId,
    string Title,
    string Slug) : ICommand<bool>;

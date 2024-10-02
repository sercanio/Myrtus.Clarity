using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Blogs.Commands.CreateBlog;

public sealed record CreateBlogCommand(
    string Title,
    string Slug,
    Guid UserId) : ICommand<Guid>;

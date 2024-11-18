using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Features.Blogs.Commands.Create;

public sealed record CreateBlogCommand(
    string Title,
    string Slug,
    Guid UserId) : ICommand<CreateBlogCommandResponse>;

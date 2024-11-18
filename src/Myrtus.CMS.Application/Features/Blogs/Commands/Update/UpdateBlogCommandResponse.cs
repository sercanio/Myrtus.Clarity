namespace Myrtus.CMS.Application.Features.Blogs.Commands.Update;

public sealed record UpdateBlogCommandResponse(
        Guid Id,
        string Title,
        string Slug,
        Guid OwnerId,
        DateTime UpdatedOnUtc);

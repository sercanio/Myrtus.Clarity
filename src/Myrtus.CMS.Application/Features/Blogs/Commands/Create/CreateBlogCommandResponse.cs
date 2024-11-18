namespace Myrtus.CMS.Application.Features.Blogs.Commands.Create;

public sealed record CreateBlogCommandResponse(
 Guid Id,
 string Title,
 string Slug,
 Guid OwnerId,
 DateTime CreatedOnUtc);

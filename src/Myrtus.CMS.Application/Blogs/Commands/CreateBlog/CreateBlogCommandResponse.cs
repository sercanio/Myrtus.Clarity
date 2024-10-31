namespace Myrtus.CMS.Application.Blogs.Commands.CreateBlog;

public sealed record CreateBlogCommandResponse(
 Guid Id,
 string Title,
 string Slug,
 Guid OwnerId,
 DateTime CreatedOnUtc);

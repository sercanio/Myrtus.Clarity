namespace Myrtus.CMS.Application.Blogs.Commands.UpdateBlog;

public sealed record UpdateBlogCommandResponse(
 Guid Id,
 string Title,
 string Slug,
 Guid OwnerId, 
 DateTime UpdatedOnUtc
);

namespace Myrtus.CMS.Application.Blogs.Commands.DeleteBlog;

public sealed record DeleteBlogCommandResponse(
 Guid Id,
 string Title,
 string Slug
);

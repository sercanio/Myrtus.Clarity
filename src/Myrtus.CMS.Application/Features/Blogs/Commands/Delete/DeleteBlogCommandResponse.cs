namespace Myrtus.CMS.Application.Features.Blogs.Commands.Delete;

public sealed record DeleteBlogCommandResponse(
 Guid Id,
 string Title,
 string Slug);

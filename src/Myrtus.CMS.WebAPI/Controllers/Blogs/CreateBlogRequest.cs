namespace Myrtus.CMS.Api.Controllers.Blogs;

public sealed record CreateBlogRequest(
    string Title,
    string Slug,
    Guid UserId);

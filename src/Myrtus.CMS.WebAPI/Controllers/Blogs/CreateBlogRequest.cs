namespace Myrtus.CMS.WebAPI.Controllers.Blogs;

public sealed record CreateBlogRequest(
    string Title,
    string Slug,
    Guid UserId);

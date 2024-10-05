namespace Myrtus.CMS.Api.Controllers.Blogs;

public sealed class UpdateBlogRequest
{
    public string Title { get; init; }
    public string Slug { get; init; }
    public Guid UpdatedById { get; init; }
    public string? Description { get; init; }
}

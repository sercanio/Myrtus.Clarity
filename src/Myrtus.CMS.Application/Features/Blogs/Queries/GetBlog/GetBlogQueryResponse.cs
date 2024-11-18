using Myrtus.CMS.Application.Features.Users.Queries.GetUser;

namespace Myrtus.CMS.Application.Features.Blogs.Queries.GetBlog;

public sealed record GetBlogQueryResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Slug { get; init; }
    public GetUserQueryResponse Owner { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? UpdatedOnUtc { get; init; }
    public DateTime? DeletedOnUtc { get; init; }


    public GetBlogQueryResponse(Guid id, string title, string slug, GetUserQueryResponse owner, DateTime createdOnUtc, DateTime? updatedOnUtc, DateTime? deletedOnUtc)
    {
        Id = id;
        Title = title;
        Slug = slug;
        Owner = owner;
        CreatedOnUtc = createdOnUtc;
        UpdatedOnUtc = updatedOnUtc;
        DeletedOnUtc = deletedOnUtc;
    }
    private GetBlogQueryResponse() { }
}

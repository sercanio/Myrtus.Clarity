using Myrtus.CMS.Application.Users.Queries.GetUser;

namespace Myrtus.CMS.Application.Blogs.Queries.GetAllBlogs;

public sealed record GetAllBlogQueryResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Slug { get; init; }
    public Guid OwnerId { get; init; }
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? UpdatedOnUtc { get; init; }
    public DateTime? DeletedOnUtc { get; init; }


    public GetAllBlogQueryResponse(Guid id, string title, string slug, Guid ownerId, DateTime createdOnUtc, DateTime? updatedOnUtc, DateTime? deletedOnUtc)
    {
        Id = id;
        Title = title;
        Slug = slug;
        OwnerId = ownerId;
        CreatedOnUtc = createdOnUtc;
        UpdatedOnUtc = updatedOnUtc;
        DeletedOnUtc = deletedOnUtc;
    }
    private GetAllBlogQueryResponse() { }
}

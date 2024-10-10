using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Blogs.Posts;

public static class BlogErrors
{
    public static readonly Error NotFound = new(
        "Post.Found",
        404,
        "The post with the specified identifier was not found");

    public static readonly Error Overlap = new(
        "Post.Overlap",
        409,
        "The current post is overlapping with an existing one");

    public static readonly Error NotReviewed = new(
        "Post.NotReviewed",
        400,
        "The post is not reviewed by the editor");

    public static readonly Error AlreadyPublished = new(
        "Post.AlreadyPublished",
        409,
        "The post has already published");
}

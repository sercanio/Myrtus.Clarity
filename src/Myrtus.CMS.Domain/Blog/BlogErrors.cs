using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Blogs;

public static class BlogErrors
{
    public static readonly Error NotFound = new(
        "Blog.NotFound",
        "The blog with the specified identifier was not found");

    public static readonly Error Overlap = new(
        "Blog.Overlap",
        "The current blog is overlapping with an existing one");
}

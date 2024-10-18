using Myrtus.Clarity.Core.Application.Abstractions.Caching;

namespace Myrtus.CMS.Application.Blogs.Queries.GetBlog;

public sealed record GetBlogQuery(Guid BlogId) : ICachedQuery<BlogResponse>
{
    public string CacheKey => $"blogs-{BlogId}";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}

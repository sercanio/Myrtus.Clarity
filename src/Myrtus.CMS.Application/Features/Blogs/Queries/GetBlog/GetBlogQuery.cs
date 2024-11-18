using Myrtus.Clarity.Core.Application.Abstractions.Caching;

namespace Myrtus.CMS.Application.Features.Blogs.Queries.GetBlog;

public sealed record GetBlogQuery(Guid BlogId) : ICachedQuery<GetBlogQueryResponse>
{
    public string CacheKey => $"blogs-{BlogId}";

    public TimeSpan? Expiration => null;
}

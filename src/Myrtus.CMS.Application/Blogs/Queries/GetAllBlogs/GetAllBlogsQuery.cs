using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Blogs.Queries.GetBlog;

namespace Myrtus.CMS.Application.Blogs.Queries.GetAllBlogs;

public class GetAllBlogsQuery : IQuery<IEnumerable<BlogResponse>>
{
    public bool IncludeSoftDeleted { get; }

    public GetAllBlogsQuery(bool includeSoftDeleted = false)
    {
        IncludeSoftDeleted = includeSoftDeleted;
    }
}

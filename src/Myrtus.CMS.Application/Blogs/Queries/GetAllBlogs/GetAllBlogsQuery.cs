using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Blogs.Queries.GetBlog;

namespace Myrtus.CMS.Application.Blogs.Queries.GetAllBlogs;

public class GetAllBlogsQuery : IQuery<IPaginatedList<BlogResponse>>
{
    public bool IncludeSoftDeleted { get; }
    public int PageIndex { get; }
    public int PageSize { get; }

    public GetAllBlogsQuery(int pageIndex, int pageSize, bool includeSoftDeleted = false)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        IncludeSoftDeleted = includeSoftDeleted;
    }
}
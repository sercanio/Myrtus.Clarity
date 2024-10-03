using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.CMS.Application.Blogs.Queries.GetBlog;

namespace Myrtus.CMS.Application.Blogs.Queries.GetAllBlogs;

public class GetAllBlogsQuery : IQuery<IPaginatedList<BlogResponse>>
{
    public int PageIndex { get; }
    public int PageSize { get; }

    public GetAllBlogsQuery(int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
    }
}
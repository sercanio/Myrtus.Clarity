using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.CMS.Application.Blogs.Queries.GetBlog;

namespace Myrtus.CMS.Application.Blogs.Queries.GetAllBlogs;

public sealed record GetAllBlogsQuery(
    int PageIndex, 
    int PageSize) : IQuery<IPaginatedList<GetBlogQueryResponse>>;
using System.Data;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Application.Blogs.Queries.GetBlog;

namespace Myrtus.CMS.Application.Blogs.Queries.GetAllBlogs;

public sealed class GetAllBlogsQueryHandler : IRequestHandler<GetAllBlogsQuery, Result<IPaginatedList<BlogResponse>>>
{
    private readonly IBlogRepository _blogRepository;

    public GetAllBlogsQueryHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<Result<IPaginatedList<BlogResponse>>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
    {
        var paginatedBlogs = await _blogRepository.GetAllAsync(
            includeSoftDeleted: request.IncludeSoftDeleted,
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            cancellationToken: cancellationToken);

        var blogResponses = paginatedBlogs.Items.Select(blog => new BlogResponse
        {
            Id = blog.Id,
            Title = blog.Title?.Value ?? "Default Title", // Fallback for Title
            Slug = blog.Slug?.Value ?? "default-slug", // Fallback for Slug
            OwnerId = blog.Owner?.Id ?? Guid.Empty, // Fallback for OwnerId
            CreatedOnUtc = blog.CreatedOnUtc,
            UpdatedOnUtc = blog.UpdatedOnUtc ?? DateTime.UtcNow // TODO: Handle this.
        }).ToList();

        var paginatedList = new PaginatedList<BlogResponse>(blogResponses, paginatedBlogs.TotalCount, request.PageIndex, request.PageSize);

        return Result.Success<IPaginatedList<BlogResponse>>(paginatedList);
    }
}

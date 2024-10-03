using System.Data;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Blogs.Queries.GetBlog;
using Myrtus.CMS.Application.Repositories;

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
        try
        {
            var paginatedBlogs = await _blogRepository.GetAllAsync(
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken,
                include: blog => blog.Owner);

            var blogResponses = paginatedBlogs.Items.Select(blog => new BlogResponse
            {
                Id = blog.Id,
                Title = blog.Title.Value,
                Slug = blog.Slug.Value,
                OwnerId = blog.Owner.Id,
                CreatedOnUtc = blog.CreatedOnUtc,
                UpdatedOnUtc = blog.UpdatedOnUtc,
                DeletedOnUtc = blog.DeletedOnUtc,
            }).ToList();

            var paginatedList = new PaginatedList<BlogResponse>(blogResponses, paginatedBlogs.TotalCount, request.PageIndex, request.PageSize);

            return Result.Success<IPaginatedList<BlogResponse>>(paginatedList);
        }
        catch (DataException ex)
        {
            // Handle specific database-related exceptions
            return Result.Failure<IPaginatedList<BlogResponse>>(new Error("DB_ERROR", "A database error occurred while retrieving the blogs."));
        }
        catch (Exception ex)
        {
            // Catch any other unexpected errors
            return Result.Failure<IPaginatedList<BlogResponse>>(new Error("UNKNOWN_ERROR", "An unexpected error occurred."));
        }
    }
}

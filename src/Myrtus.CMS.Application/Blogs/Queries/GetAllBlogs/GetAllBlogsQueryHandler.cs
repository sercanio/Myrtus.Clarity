using System.Data;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Blogs.Queries.GetBlog;
using Ardalis.Result;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Application.Users.Queries.GetUser;

namespace Myrtus.CMS.Application.Blogs.Queries.GetAllBlogs;

public sealed class GetAllBlogsQueryHandler : IRequestHandler<GetAllBlogsQuery, Result<IPaginatedList<GetBlogQueryResponse>>>
{
    private readonly IBlogRepository _blogRepository;

    public GetAllBlogsQueryHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<Result<IPaginatedList<GetBlogQueryResponse>>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
    {
        var blogs = await _blogRepository.GetAllAsync(
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            include: blog => blog.Owner,
            cancellationToken: cancellationToken);

        var mappedBlogs = blogs.Items.Select(blog =>
        {
            var ownerResponse = new GetUserQueryResponse(
                blog.Owner.Id,
                blog.Owner.Email.Value,
                blog.Owner.FirstName.Value,
                blog.Owner.LastName.Value
            );

            return new GetBlogQueryResponse(
                blog.Id,
                blog.Title.Value,
                blog.Slug.Value,
                ownerResponse,
                blog.CreatedOnUtc,
                blog.UpdatedOnUtc,
                blog.DeletedOnUtc
            );
        }).ToList();

        var paginatedList = new PaginatedList<GetBlogQueryResponse>(mappedBlogs, blogs.TotalCount, request.PageIndex, request.PageSize);

        return Result.Success<IPaginatedList<GetBlogQueryResponse>>(paginatedList);
    }
}

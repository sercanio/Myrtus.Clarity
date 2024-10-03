using MediatR;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;

namespace Myrtus.CMS.Application.Blogs.Queries.GetBlog;

public sealed class GetBlogQueryHandler : IRequestHandler<GetBlogQuery, Result<BlogResponse>>
{
    private readonly IBlogRepository _blogRepository;

    public GetBlogQueryHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }
    public async Task<Result<BlogResponse>> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetByIdAsync(
            request.BlogId, 
            include: blog => blog.Owner,
            cancellationToken: cancellationToken);

        if(blog is null)
        {
            return Result.Failure<BlogResponse>(BlogErrors.NotFound);
        }

        BlogResponse blogResponse = new()
        {
            Id = blog.Id,
            Title = blog.Title.Value,
            Slug = blog.Slug.Value,
            OwnerId = blog.Owner.Id,
            CreatedOnUtc = blog.CreatedOnUtc,
            UpdatedOnUtc = blog.UpdatedOnUtc,
            DeletedOnUtc = blog.DeletedOnUtc,
        };

        return Result.Success(blogResponse);
    }

}

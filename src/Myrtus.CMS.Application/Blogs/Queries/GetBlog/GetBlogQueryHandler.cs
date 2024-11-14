using Ardalis.Result;
using Myrtus.CMS.Application.Repositories;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.CMS.Application.Users.Queries.GetUser;
using Myrtus.CMS.Domain.Blogs;

namespace Myrtus.CMS.Application.Blogs.Queries.GetBlog;

public sealed class GetBlogQueryHandler : IQueryHandler<GetBlogQuery, GetBlogQueryResponse>
{
    private readonly IBlogRepository _blogRepository;

    public GetBlogQueryHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<Result<GetBlogQueryResponse>> Handle(GetBlogQuery request, CancellationToken cancellationToken)
    {
        // Get the blog with owner information included
        var blog = await _blogRepository.GetAsync(
            predicate: blog => blog.Id == request.BlogId,
            include: b => b.Owner,
            cancellationToken: cancellationToken);

        if (blog is null)
        {
            return Result<GetBlogQueryResponse>.NotFound(BlogErrors.NotFound.Name);
        }

        // Map the Owner entity to GetUserQueryResponse
        var ownerResponse = new GetUserQueryResponse(
            blog.Owner.Id,
            blog.Owner.Email,
            blog.Owner.FirstName,
            blog.Owner.LastName
        // Add any additional properties if needed
        );

        // Map the Blog domain entity to GetBlogQueryResponse, including the mapped owner
        var response = new GetBlogQueryResponse(
            blog.Id,
            blog.Title.Value,
            blog.Slug.Value,
            ownerResponse, // Use the mapped GetUserQueryResponse
            blog.CreatedOnUtc,
            blog.UpdatedOnUtc,
            blog.DeletedOnUtc
        );

        return Result.Success(response);
    }
}

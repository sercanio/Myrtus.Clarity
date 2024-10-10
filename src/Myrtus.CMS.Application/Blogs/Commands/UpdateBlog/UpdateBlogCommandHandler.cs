using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Blogs.Common;

namespace Myrtus.CMS.Application.Blogs.Commands.UpdateBlog;

public sealed class UpdateBlogCommandHandler : ICommandHandler<UpdateBlogCommand, UpdateBlogCommandResponse>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public UpdateBlogCommandHandler(
        IBlogRepository blogRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _blogRepository = blogRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<UpdateBlogCommandResponse>> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetBlogByIdAsync(request.BlogId, include: blog => blog.Owner, cancellationToken: cancellationToken);

        if (blog is null)
        {
            return Result.Failure<UpdateBlogCommandResponse>(BlogErrors.NotFound);
        }

        var slugAlreadyExists = await _blogRepository.BlogExistsBySlugAsync(new Slug(request.Slug), cancellationToken);
        if (slugAlreadyExists && blog.Slug.Value != request.Slug) // Ensure we're not comparing to itself
        {
            return Result.Failure<UpdateBlogCommandResponse>(BlogErrors.SlugAlreadyExists);
        }

        var titleAlreadyExists = await _blogRepository.BlogExistsByTitleAsync(new Title(request.Title), cancellationToken);
        if (titleAlreadyExists && blog.Title.Value != request.Title) // Ensure we're not comparing to itself
        {
            return Result.Failure<UpdateBlogCommandResponse>(BlogErrors.TitleAlreadyExists);
        }

        blog.ChangeTitle(new Title(request.Title));
        blog.ChangeSlug(new Slug(request.Slug));

        _blogRepository.Update(blog);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Clear the cache for this blog
        await _cacheService.RemoveAsync($"blogs-{request.BlogId}", cancellationToken);

        UpdateBlogCommandResponse response = new UpdateBlogCommandResponse(
            blog.Id,
            blog.Title.Value,
            blog.Slug.Value,
            blog.Owner.Id,
            blog.UpdatedOnUtc.Value
        );

        return Result.Success(response);
    }
}
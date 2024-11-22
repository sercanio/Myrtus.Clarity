using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;

namespace Myrtus.CMS.Application.Features.Blogs.Commands.Delete;

public class DeleteBlogCommandHandler : ICommandHandler<DeleteBlogCommand, DeleteBlogCommandResponse>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public DeleteBlogCommandHandler(
        IBlogRepository blogRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _blogRepository = blogRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<DeleteBlogCommandResponse>> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        Blog? blog = await _blogRepository.GetBlogByIdAsync(request.BlogId, cancellationToken: cancellationToken);

        if (blog is null)
        {
            return Result.NotFound(BlogErrors.NotFound.Name);
        }

        _blogRepository.Delete(blog);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"blogs-{request.BlogId}", cancellationToken);

        DeleteBlogCommandResponse response = new(
            blog.Id,
            blog.Title.Value,
            blog.Slug.Value
        );

        return Result.Success(response);
    }
}

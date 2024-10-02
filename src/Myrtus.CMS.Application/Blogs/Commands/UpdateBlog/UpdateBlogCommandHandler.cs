using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Blogs.Common;

namespace Myrtus.CMS.Application.Blogs.Commands.UpdateBlog;
public sealed class UpdateBlogCommandHandler : ICommandHandler<UpdateBlogCommand, bool>
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

    public async Task<Result<bool>> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        Blog? blog = await _blogRepository.GetByIdAsync(request.BlogId, cancellationToken);

        if (blog is null)
        {
            return Result.Failure<bool>(BlogErrors.NotFound);
        }

        blog.ChangeTitle(new Title(request.Title));
        blog.ChangeSlug(new Slug(request.Slug));

        _blogRepository.Update(blog);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cacheService.RemoveAsync($"blogs-{request.BlogId}", cancellationToken);

        return true;
    }
}

using MediatR;
using Myrtus.CMS.Application.Blogs.Commands.DeleteBlog;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;

namespace Myrtus.CMS.Application.Blogs.Commands;

public class DeleteBlogCommandHandler : ICommandHandler<DeleteBlogCommand, DeleteBlogCommandResponse>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public DeleteBlogCommandHandler(IBlogRepository blogRepository, IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _blogRepository = blogRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<DeleteBlogCommandResponse>> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetBlogByIdAsync(request.BlogId, cancellationToken: cancellationToken);

        if (blog == null)
        {
            return Result.Failure<DeleteBlogCommandResponse>(BlogErrors.NotFound);
        }

        _blogRepository.Delete(blog);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cacheService.RemoveAsync($"blogs-{request.BlogId}", cancellationToken);

        DeleteBlogCommandResponse response = new DeleteBlogCommandResponse(
            blog.Id, 
            Title: blog.Title.Value, 
            Slug: blog.Slug.Value);

        return Result.Success(response);
    }
}

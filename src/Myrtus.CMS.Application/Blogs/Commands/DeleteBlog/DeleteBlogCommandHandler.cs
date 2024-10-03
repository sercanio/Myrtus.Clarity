using MediatR;
using Myrtus.CMS.Application.Blogs.Commands.DeleteBlog;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.CMS.Application.Repositories;

namespace Myrtus.CMS.Application.Blogs.Commands;

public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand, bool>
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

    public async Task<bool> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = await _blogRepository.GetByIdAsync(request.BlogId, cancellationToken: cancellationToken);

        if (blog == null)
        {
            return false;
        }

        _blogRepository.Delete(blog);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cacheService.RemoveAsync($"blogs-{request.BlogId}", cancellationToken);

        return true;
    }
}

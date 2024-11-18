using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Commands;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;

namespace Myrtus.CMS.Application.Features.Blogs.Commands.Delete;

public class DeleteBlogCommandHandler : BaseCommandHandler<DeleteBlogCommand, DeleteBlogCommandResponse>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public DeleteBlogCommandHandler(
        IBlogRepository blogRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IAuditLogService auditLogService,
        IHttpContextAccessor httpContextAccessor) : base(auditLogService, httpContextAccessor)
    {
        _blogRepository = blogRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public override async Task<Result<DeleteBlogCommandResponse>> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
    {
        Blog? blog = await _blogRepository.GetBlogByIdAsync(request.BlogId, cancellationToken: cancellationToken);

        if (blog is null)
        {
            return Result.NotFound(BlogErrors.NotFound.Name);
        }

        _blogRepository.Delete(blog);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"blogs-{request.BlogId}", cancellationToken);

        await LogAuditAsync("DeleteBlog", "Blog", blog.Title.Value, $"Blog '{blog.Title.Value}' deleted.");

        var response = new DeleteBlogCommandResponse(
            blog.Id,
            blog.Title.Value,
            blog.Slug.Value
        );

        return Result.Success(response);
    }
}

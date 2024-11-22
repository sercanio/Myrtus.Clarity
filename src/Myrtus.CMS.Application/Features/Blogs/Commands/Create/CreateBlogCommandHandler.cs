using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Blogs.Common;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Features.Blogs.Commands.Create;

public sealed class CreateBlogCommandHandler : ICommandHandler<CreateBlogCommand, CreateBlogCommandResponse>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBlogCommandHandler(
        IBlogRepository blogRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _blogRepository = blogRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateBlogCommandResponse>> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken: cancellationToken);

        if (user is null)
        {
            return Result.NotFound(UserErrors.NotFound.Name);
        }

        var title = new Title(request.Title);
        bool titleExists = await _blogRepository.BlogExistsByTitleAsync(title, cancellationToken);
        if (titleExists)
        {
            return Result.Conflict(BlogErrors.TitleAlreadyExists.Name);
        }

        var slug = new Slug(request.Slug);
        bool slugExists = await _blogRepository.BlogExistsBySlugAsync(slug, cancellationToken);
        if (slugExists)
        {
            return Result.Conflict(BlogErrors.SlugAlreadyExists.Name);
        }

        var blog = Blog.Create(title, slug, user);

        await _blogRepository.AddAsync(blog);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        CreateBlogCommandResponse response = new(
            blog.Id,
            blog.Title.Value,
            blog.Slug.Value,
            blog.Owner.Id,
            blog.CreatedOnUtc);

        return Result.Success(response);
    }
}

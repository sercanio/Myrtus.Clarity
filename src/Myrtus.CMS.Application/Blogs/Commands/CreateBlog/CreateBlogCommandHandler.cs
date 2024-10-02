using Myrtus.Clarity.Core.Application.Abstractions.Clock;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Blogs.Common;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Blogs.Commands.CreateBlog;

internal sealed class CreateBlogCommandHandler : ICommandHandler<CreateBlogCommand, Guid>
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

    public async Task<Result<Guid>> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var blog = Blog.Create(
            new Title(request.Title),
            new Slug(request.Slug),
            user);

        _blogRepository.Add(blog);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return blog.Id;
    }
}

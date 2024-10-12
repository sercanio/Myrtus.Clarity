using FluentValidation;

namespace Myrtus.CMS.Application.Blogs.Commands.DeleteBlog;

public class DeleteBlogCommandValidator : AbstractValidator<DeleteBlogCommand>
{
    public DeleteBlogCommandValidator()
    {
        RuleFor(c => c.BlogId).NotEmpty();
    }
}
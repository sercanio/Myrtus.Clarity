using FluentValidation;

namespace Myrtus.CMS.Application.Features.Blogs.Commands.Delete;

public class DeleteBlogCommandValidator : AbstractValidator<DeleteBlogCommand>
{
    public DeleteBlogCommandValidator()
    {
        RuleFor(c => c.BlogId).NotEmpty();
    }
}
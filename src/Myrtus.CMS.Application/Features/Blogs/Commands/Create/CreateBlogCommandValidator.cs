using FluentValidation;

namespace Myrtus.CMS.Application.Features.Blogs.Commands.Create;

public class CreateBlogCommandValidator : AbstractValidator<CreateBlogCommand>
{
    public CreateBlogCommandValidator()
    {
        RuleFor(c => c.Title).NotEmpty().MaximumLength(45);
        RuleFor(c => c.Slug).NotEmpty().MaximumLength(45);
        RuleFor(c => c.UserId).NotEmpty();
    }
}
using FluentValidation;
using System.Text.RegularExpressions;

namespace Myrtus.CMS.Application.Features.Blogs.Commands.Update;

public class UpdateBlogCommandValidator : AbstractValidator<UpdateBlogCommand>
{
    private static readonly Regex SlugRegex = new Regex("^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.IgnoreCase);

    public UpdateBlogCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotNull()
            .NotEmpty()
            .MaximumLength(45);
        RuleFor(command => command.Slug)
            .NotNull()
            .NotEmpty()
            .MaximumLength(45)
            .Must(BeAValidSlug).WithMessage("Slug contains invalid characters.");
        RuleFor(c => c.UpdatedById)
            .NotNull()
            .NotEmpty();
    }

    private bool BeAValidSlug(string slug)
    {
        return SlugRegex.IsMatch(slug);
    }
}
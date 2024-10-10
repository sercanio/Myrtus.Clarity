using FluentValidation;
using Myrtus.CMS.Application.Blogs.Commands.DeleteBlog;

namespace Myrtus.CMS.Application.Blogs.Commands.CreateBlog;

public class DeleteBlogCommandValidator : AbstractValidator<DeleteBlogCommand>
{
    public DeleteBlogCommandValidator()
    {
        RuleFor(c => c.BlogId).NotEmpty();
    }
}
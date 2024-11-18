using FluentValidation;

namespace Myrtus.CMS.Application.Features.Blogs.Queries.GetBlog;

public class GetBlogQueryValidator : AbstractValidator<GetBlogQuery>
{
    public GetBlogQueryValidator()
    {
        RuleFor(c => c.BlogId).NotEmpty();
    }
}
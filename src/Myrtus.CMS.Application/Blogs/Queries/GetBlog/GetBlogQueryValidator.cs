using FluentValidation;

namespace Myrtus.CMS.Application.Blogs.Queries.GetBlog;

public class GetBlogQueryValidator : AbstractValidator<GetBlogQuery>
{
    public GetBlogQueryValidator()
    {
        RuleFor(c => c.BlogId).NotEmpty();
    }
}
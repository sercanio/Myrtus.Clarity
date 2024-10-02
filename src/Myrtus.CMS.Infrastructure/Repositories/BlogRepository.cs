using Myrtus.CMS.Domain.Blogs;
using Microsoft.EntityFrameworkCore;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using System.Linq.Expressions;
using Myrtus.CMS.Application.Repositories;

namespace Myrtus.CMS.Infrastructure.Repositories;

internal sealed class BlogRepository : Repository<Blog>, IBlogRepository
{
    public BlogRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<bool> BlogExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<Blog>()
            .AnyAsync(blog => blog.Id == id && blog.DeletedOnUtc == null, cancellationToken);
    }

    public override void Add(Blog blog)
    {
        DbContext.Add(blog);
    }

    public override void Update(Blog blog)
    {
        DbContext.Update(blog);
    }

    public override void Delete(Blog blog)
    {
        blog.MarkDeleted();
        DbContext.Update(blog);
    }
}
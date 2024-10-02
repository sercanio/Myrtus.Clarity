using Myrtus.CMS.Domain.Blogs;
using Microsoft.EntityFrameworkCore;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;

namespace Myrtus.CMS.Infrastructure.Repositories;

internal sealed class BlogRepository : Repository<Blog>, IBlogRepository
{
    public BlogRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<IPaginatedList<Blog>> GetAllAsync(bool includeSoftDeleted = false, int pageIndex = 0, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Set<Blog>().AsQueryable();

        if (!includeSoftDeleted)
        {
            query = query.Where(blog => blog.DeletedOnUtc == null);
        }

        var count = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageIndex) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<Blog>(items, count, pageIndex, pageSize);
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

    public void Update(Blog blog)
    {
        DbContext.Update(blog);
    }

    public void Delete(Blog blog)
    {
        blog.MarkDeleted();
        DbContext.Update(blog);
    }
}

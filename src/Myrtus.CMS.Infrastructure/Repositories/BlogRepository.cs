using System.Linq.Expressions;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs.Common;
using Myrtus.CMS.Domain.Blogs.Events;

namespace Myrtus.CMS.Infrastructure.Repositories;

internal sealed class BlogRepository : Repository<Blog>, IBlogRepository
{
    public BlogRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<Blog?> GetBlogByIdAsync(
        Guid id,
        bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<Blog, object>>[] include)
    {
        return await GetAsync(blog => blog.Id == id, includeSoftDeleted, cancellationToken, include);
    }

    public async Task<bool> BlogExistsByIdAsync(
             Guid id,
             CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(blog => blog.Id == id, cancellationToken:cancellationToken);
    }

    public async Task<bool> BlogExistsByTitleAsync(Title title, CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(blog => blog.Title == title, cancellationToken: cancellationToken);
    }

    public async Task<bool> BlogExistsBySlugAsync(Slug slug, CancellationToken cancellationToken = default)
    {
        return await ExistsAsync(blog => blog.Slug == slug, cancellationToken: cancellationToken);
    }

    public override void Add(Blog blog)
    {
        DbContext.Add(blog);
        blog.RaiseDomainEvent(new BlogCreatedEvent(blog.Id));
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

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.Clarity.Core.Application.Repositories;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;

namespace Myrtus.CMS.Infrastructure.Repositories;

internal abstract class Repository<T> : IRepository<T>
    where T : Entity
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<T?> GetByIdAsync(
        Guid id,
        bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] include)
    {
        var query = DbContext.Set<T>().AsQueryable();

        // Exclude soft-deleted entities if `includeSoftDeleted` is false and 'DeletedOnUtc' property exists
        if (!includeSoftDeleted && typeof(T).GetProperty("DeletedOnUtc") != null)
        {
            var parameter = Expression.Parameter(typeof(T), "entity");
            var property = Expression.Property(parameter, "DeletedOnUtc");
            var comparison = Expression.Equal(property, Expression.Constant(null));
            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);

            query = query.Where(lambda);
        }

        foreach (var item in include)
        {
            query = query.Include(item);
        }

        return await query.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task<IPaginatedList<T>> GetAllAsync(
        int pageIndex = 0,
        int pageSize = 10,
        bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] include)
    {
        var query = DbContext.Set<T>().AsQueryable();

        if (!includeSoftDeleted && typeof(T).GetProperty("DeletedOnUtc") != null)
        {
            var parameter = Expression.Parameter(typeof(T), "entity");
            var property = Expression.Property(parameter, "DeletedOnUtc");
            var comparison = Expression.Equal(property, Expression.Constant(null));
            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);

            query = query.Where(lambda);
        }

        foreach (var item in include)
        {
            query = query.Include(item);
        }

        var count = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }

    public virtual void Add(T entity)
    {
        DbContext.Add(entity);
    }

    public virtual void Update(T entity)
    {
        DbContext.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        var property = typeof(T).GetProperty("MarkDeleted");
        if (property != null)
        {
            property.SetValue(entity, null);
        }

        DbContext.Update(entity);
    }
}

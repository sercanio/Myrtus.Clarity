using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.Clarity.Core.Application.Repositories;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;
using System.Linq.Dynamic.Core;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Infrastructure.Repositories
{
    internal abstract class Repository<T> : IRepository<T>
        where T : Entity
    {
        protected readonly ApplicationDbContext DbContext;

        protected Repository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<T?> GetAsync(
            Expression<Func<T, bool>> predicate,
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

            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
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

        public async Task<IPaginatedList<T>> GetAllDynamicAsync(
            DynamicQuery dynamicQuery,
            int pageIndex = 0,
            int pageSize = 10,
            bool includeSoftDeleted = false,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] include)
        {
            var query = DbContext.Set<T>().AsQueryable();

            // Handle soft delete filtering
            if (!includeSoftDeleted && typeof(T).GetProperty("DeletedOnUtc") != null)
            {
                var parameter = Expression.Parameter(typeof(T), "entity");
                var property = Expression.Property(parameter, "DeletedOnUtc");
                var comparison = Expression.Equal(property, Expression.Constant(null));
                var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
                query = query.Where(lambda);
            }

            // Include related entities
            foreach (var includeExpression in include)
            {
                query = query.Include(includeExpression);
            }

            // Apply dynamic query (filtering and sorting)
            query = query.ToDynamic(dynamicQuery);

            // Get total count before pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var items = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, totalCount, pageIndex, pageSize);
        }

        public virtual async Task<bool> ExistsAsync(
            Expression<Func<T, bool>> predicate,
            bool includeSoftDeleted = false,
            CancellationToken cancellationToken = default)
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

            return await query.AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task AddAsync(T entity)
        {
            await DbContext.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            DbContext.Update(entity);
        }

        public virtual void Delete(T entity, bool isSoftDelete = true)
        {
            var property = typeof(T).GetProperty("MarkDeleted");
            if (property != null)
            {
                property.SetValue(entity, null);
            }

            DbContext.Update(entity);
        }
    }
}

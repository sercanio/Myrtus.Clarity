using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using System.Linq.Expressions;

namespace Myrtus.CMS.Application.Repositories;

public interface IRepository<T>
{
    Task<T?> GetByIdAsync(
        Guid id,
        bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] include);

    Task<IPaginatedList<T>> GetAllAsync(
        int pageIndex = 0,
        int pageSize = 10,
        bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] include);

    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}

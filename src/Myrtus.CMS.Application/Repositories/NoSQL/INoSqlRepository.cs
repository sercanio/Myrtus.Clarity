using System.Linq.Expressions;

namespace Myrtus.CMS.Application.Repositories.NoSQL
{
    public interface INoSqlRepository<T>
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    }
}

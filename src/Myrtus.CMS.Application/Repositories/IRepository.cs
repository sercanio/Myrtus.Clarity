using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.CMS.Domain.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Myrtus.CMS.Application.Repositories;

public interface IRepository<T>
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] include);
    Task<IPaginatedList<T>> GetAllAsync(
        bool includeSoftDeleted = false,
        int pageIndex = 0,
        int pageSize = 10,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] include);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}

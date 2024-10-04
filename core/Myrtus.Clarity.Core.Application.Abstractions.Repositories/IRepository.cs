﻿using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using System.Linq.Expressions;

namespace Myrtus.Clarity.Core.Application.Repositories;

public interface IRepository<T>
{    Task<T?> GetAsync(
        Expression<Func<T, bool>> predicate,
        bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] include);

    Task<IPaginatedList<T>> GetAllAsync(
        int pageIndex = 0,
        int pageSize = 10,
        bool includeSoftDeleted = false,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] include);

    Task<bool> ExistsAsync(
            Expression<Func<T, bool>> predicate,
            bool includeSoftDeleted = false,
            CancellationToken cancellationToken = default);

    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}

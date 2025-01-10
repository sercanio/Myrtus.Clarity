using MongoDB.Driver;
using Myrtus.Clarity.Application.Repositories.NoSQL;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using System.Linq.Expressions;

namespace Myrtus.Clarity.Infrastructure.Repositories.NoSQL
{
    public class NoSqlRepository<T> : INoSqlRepository<T>
    {
        protected readonly IMongoCollection<T> _collection;

        public NoSqlRepository(IMongoDatabase database, string collectionName)
        {
            _collection = database.GetCollection<T>(collectionName);
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IPaginatedList<T>> GetAllAsync(int pageIndex = 0, int pageSize = 10, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var filter = predicate ?? (_ => true);
            var totalCount = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

            var items = await _collection.Find(filter)
                .Skip(pageIndex * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedList<T>(items, (int)totalCount, pageIndex, pageSize);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(entity, null, cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq("Id", entity.GetType().GetProperty("Id")?.GetValue(entity, null));
            await _collection.ReplaceOneAsync(filter, entity, new ReplaceOptions(), cancellationToken);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            await _collection.DeleteOneAsync(predicate, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(predicate).ToListAsync(cancellationToken);
        }
    }
}

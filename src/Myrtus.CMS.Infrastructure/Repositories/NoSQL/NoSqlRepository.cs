using MongoDB.Driver;
using Myrtus.CMS.Application.Repositories.NoSQL;
using System.Linq.Expressions;

namespace Myrtus.CMS.Infrastructure.Repositories.NoSQL
{
    public class NoSqlRepository<T>(IMongoDatabase database, string collectionName) : INoSqlRepository<T>
    {
        private readonly IMongoCollection<T> _collection = database.GetCollection<T>(collectionName);

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(entity, null, cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            FilterDefinition<T> filter = Builders<T>.Filter.Eq("Id", entity.GetType().GetProperty("Id")?.GetValue(entity, null));
            await _collection.ReplaceOneAsync(filter, entity, new ReplaceOptions(), cancellationToken);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            await _collection.DeleteOneAsync(predicate, cancellationToken);
        }
    }
}

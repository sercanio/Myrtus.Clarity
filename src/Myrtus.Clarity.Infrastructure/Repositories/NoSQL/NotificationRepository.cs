﻿using MongoDB.Driver;
using Myrtus.Clarity.Application.Repositories.NoSQL;
using Myrtus.Clarity.Core.Domain.Abstractions;
using System.Linq.Expressions;

namespace Myrtus.Clarity.Infrastructure.Repositories.NoSQL
{
    internal sealed class NotificationRepository : NoSqlRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(IMongoDatabase database)
            : base(database, "Notifications")
        {
        }

        public async Task<IEnumerable<Notification>> GetByPredicateAsync(Expression<Func<Notification, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(predicate).ToListAsync(cancellationToken);
        }
    }
}

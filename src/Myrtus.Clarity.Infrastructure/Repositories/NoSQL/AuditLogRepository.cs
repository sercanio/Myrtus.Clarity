using MongoDB.Driver;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Application.Repositories.NoSQL;

namespace Myrtus.Clarity.Infrastructure.Repositories.NoSQL
{
    internal sealed class AuditLogRepository : NoSqlRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(IMongoDatabase database)
            : base(database, "AuditLogs")
        {
        }
    }
}

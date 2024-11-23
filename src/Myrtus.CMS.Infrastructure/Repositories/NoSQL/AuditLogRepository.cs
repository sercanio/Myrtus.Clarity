using MongoDB.Driver;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories.NoSQL;

namespace Myrtus.CMS.Infrastructure.Repositories.NoSQL
{
    internal sealed class AuditLogRepository : NoSqlRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(IMongoDatabase database)
            : base(database, "AuditLogs")
        {
        }
    }
}

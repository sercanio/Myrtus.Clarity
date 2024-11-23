using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Application.Repositories.NoSQL
{
    public interface IAuditLogRepository : INoSqlRepository<AuditLog>
    {
    }
}

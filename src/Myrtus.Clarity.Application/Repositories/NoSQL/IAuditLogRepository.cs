﻿using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.Clarity.Application.Repositories.NoSQL
{
    public interface IAuditLogRepository : INoSqlRepository<AuditLog>
    {
    }
}

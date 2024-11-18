using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myrtus.CMS.Application.Features.AuditLogs.Queries;

public sealed record GetAllAuditLogsQueryResponse(
    Guid Id,
    string User,
    string Action,
    string Entity,
    string EntityId,
    DateTime Timestamp,
    string Details
);

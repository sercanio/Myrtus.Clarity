using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.CMS.Application.Features.Roles.Queries.GetAllRoles;

namespace Myrtus.CMS.Application.Features.AuditLogs.Queries.GetAllAuditLogs;

public sealed record GetAllAuditLogsQuery(
    int PageIndex,
    int PageSize,
    CancellationToken CancellationToken) : IQuery<IPaginatedList<GetAllAuditLogsQueryResponse>>;

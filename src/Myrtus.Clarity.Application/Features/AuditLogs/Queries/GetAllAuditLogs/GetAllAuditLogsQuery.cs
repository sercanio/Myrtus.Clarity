using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;

namespace Myrtus.Clarity.Application.Features.AuditLogs.Queries.GetAllAuditLogs
{
    public sealed record GetAllAuditLogsQuery(
        int PageIndex,
        int PageSize,
        CancellationToken CancellationToken) : IQuery<IPaginatedList<GetAllAuditLogsQueryResponse>>;
}

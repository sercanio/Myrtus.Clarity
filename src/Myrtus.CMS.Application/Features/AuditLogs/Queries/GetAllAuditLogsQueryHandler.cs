using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Repositories.NoSQL;

namespace Myrtus.CMS.Application.Features.AuditLogs.Queries;

public class GetAllAuditLogsQueryHandler : IRequestHandler<GetAllAuditLogsQuery, Result<IPaginatedList<GetAllAuditLogsQueryResponse>>>
{
    private readonly INoSqlRepository<AuditLog> _auditLogRepository;

    public GetAllAuditLogsQueryHandler(INoSqlRepository<AuditLog> auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    public async Task<Result<IPaginatedList<GetAllAuditLogsQueryResponse>>> Handle(GetAllAuditLogsQuery request, CancellationToken cancellationToken)
    {
        var auditLogs = await _auditLogRepository.GetAllAsync(cancellationToken);

        var paginatedAuditLogs = auditLogs
            .Skip(request.PageIndex * request.PageSize)
            .Take(request.PageSize)
            .Select(auditLog => new GetAllAuditLogsQueryResponse(
                auditLog.Id,
                auditLog.User,
                auditLog.Action,
                auditLog.Entity,
                auditLog.EntityId,
                auditLog.Timestamp,
                auditLog.Details
            ))
            .ToList();

        var paginatedList = new PaginatedList<GetAllAuditLogsQueryResponse>(
            paginatedAuditLogs,
            auditLogs.Count(),
            request.PageIndex,
            request.PageSize
        );

        return Result.Success<IPaginatedList<GetAllAuditLogsQueryResponse>>(paginatedList);
    }
}

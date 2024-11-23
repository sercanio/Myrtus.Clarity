using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.Clarity.Application.Repositories.NoSQL;

namespace Myrtus.Clarity.Application.Features.AuditLogs.Queries.GetAllAuditLogs
{
    public class GetAllAuditLogsQueryHandler(INoSqlRepository<AuditLog> auditLogRepository) : IRequestHandler<GetAllAuditLogsQuery, Result<IPaginatedList<GetAllAuditLogsQueryResponse>>>
    {
        private readonly INoSqlRepository<AuditLog> _auditLogRepository = auditLogRepository;

        public async Task<Result<IPaginatedList<GetAllAuditLogsQueryResponse>>> Handle(GetAllAuditLogsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<AuditLog> auditLogs = await _auditLogRepository.GetAllAsync(cancellationToken);

            List<GetAllAuditLogsQueryResponse> paginatedAuditLogs = auditLogs
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

            PaginatedList<GetAllAuditLogsQueryResponse> paginatedList = new(
                paginatedAuditLogs,
                auditLogs.Count(),
                request.PageIndex,
                request.PageSize
            );

            return Result.Success<IPaginatedList<GetAllAuditLogsQueryResponse>>(paginatedList);
        }
    }
}

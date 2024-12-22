using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.Clarity.Application.Repositories.NoSQL;

namespace Myrtus.Clarity.Application.Features.AuditLogs.Queries.GetAllAuditLogsDynamic
{
    public class GetAllAuditLogsDynamicQueryHandler(INoSqlRepository<AuditLog> auditLogRepository) 
        : IRequestHandler<GetAllAuditLogsDynamicQuery, Result<IPaginatedList<GetAllAuditLogsDynamicQueryResponse>>>
    {
        private readonly INoSqlRepository<AuditLog> _auditLogRepository = auditLogRepository;

        public async Task<Result<IPaginatedList<GetAllAuditLogsDynamicQueryResponse>>> Handle(
            GetAllAuditLogsDynamicQuery request, 
            CancellationToken cancellationToken)
        {
            IEnumerable<AuditLog> auditLogs = await _auditLogRepository.GetAllAsync(
                            predicate: null,
                            cancellationToken: cancellationToken);

            IQueryable<AuditLog> filteredAuditLogs = auditLogs.AsQueryable().ToDynamic(request.DynamicQuery);

            List<GetAllAuditLogsDynamicQueryResponse> paginatedAuditLogs = [.. filteredAuditLogs
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize)
                .Select(auditLog => new GetAllAuditLogsDynamicQueryResponse(
                    auditLog.Id,
                    auditLog.User,
                    auditLog.Action,
                    auditLog.Entity,
                    auditLog.EntityId,
                    auditLog.Timestamp,
                    auditLog.Details
                ))];

            PaginatedList<GetAllAuditLogsDynamicQueryResponse> paginatedList = new(
                paginatedAuditLogs,
                filteredAuditLogs.Count(),
                request.PageIndex,
                request.PageSize
            );

            return Result.Success<IPaginatedList<GetAllAuditLogsDynamicQueryResponse>>(paginatedList);
        }
    }
}

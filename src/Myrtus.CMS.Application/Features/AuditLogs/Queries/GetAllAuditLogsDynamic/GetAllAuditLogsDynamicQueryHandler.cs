using Ardalis.Result;
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using Myrtus.CMS.Application.Repositories.NoSQL;

namespace Myrtus.CMS.Application.Features.AuditLogs.Queries.GetAllAuditLogsDynamic
{
    public class GetAllAuditLogsDynamicQueryHandler : IRequestHandler<GetAllAuditLogsDynamicQuery, Result<IPaginatedList<GetAllAuditLogsDynamicQueryResponse>>>
    {
        private readonly INoSqlRepository<AuditLog> _auditLogRepository;

        public GetAllAuditLogsDynamicQueryHandler(INoSqlRepository<AuditLog> auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<Result<IPaginatedList<GetAllAuditLogsDynamicQueryResponse>>> Handle(GetAllAuditLogsDynamicQuery request, CancellationToken cancellationToken)
        {
            var auditLogs = await _auditLogRepository.GetAllAsync(cancellationToken);

            // Apply dynamic query (filtering and sorting)
            var filteredAuditLogs = auditLogs.AsQueryable().ToDynamic(request.DynamicQuery);

            var paginatedAuditLogs = filteredAuditLogs
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
                ))
                .ToList();

            var paginatedList = new PaginatedList<GetAllAuditLogsDynamicQueryResponse>(
                paginatedAuditLogs,
                filteredAuditLogs.Count(),
                request.PageIndex,
                request.PageSize
            );

            return Result.Success<IPaginatedList<GetAllAuditLogsDynamicQueryResponse>>(paginatedList);
        }
    }
}

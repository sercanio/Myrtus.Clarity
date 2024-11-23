using Ardalis.Result;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;
using Myrtus.Clarity.Core.WebApi;
using Myrtus.CMS.Application.Features.AuditLogs.Queries.GetAllAuditLogs;
using Myrtus.CMS.Application.Features.AuditLogs.Queries.GetAllAuditLogsDynamic;
using Myrtus.CMS.WebAPI.Attributes;

namespace Myrtus.CMS.WebAPI.Controllers.AuditLogs
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/auditlogs")]
    public class AuditLogsController(ISender sender, IErrorHandlingService errorHandlingService) : ControllerBase
    {
        private readonly ISender _sender = sender;
        private readonly IErrorHandlingService _errorHandlingService = errorHandlingService;

        [HttpGet]
        public async Task<IActionResult> GetAllAuditLogs(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            GetAllAuditLogsQuery query = new(pageIndex, pageSize, cancellationToken);
            Result<IPaginatedList<GetAllAuditLogsQueryResponse>> result = await _sender.Send(query, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }

        [HttpPost("dynamic")]
        [HasPermission(Permissions.AuditLogsRead)]
        public async Task<IActionResult> GetAllAuditLogsDynamic(
            [FromBody] DynamicQuery dynamicQuery,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            GetAllAuditLogsDynamicQuery query = new(pageIndex, pageSize, dynamicQuery);
            Result<IPaginatedList<GetAllAuditLogsDynamicQueryResponse>> result = await _sender.Send(query, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }
    }
}

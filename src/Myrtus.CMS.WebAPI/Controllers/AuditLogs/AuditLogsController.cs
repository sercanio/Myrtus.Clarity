using Ardalis.Result;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.WebApi;
using Myrtus.CMS.Application.Features.AuditLogs.Queries;
using Myrtus.CMS.Application.Features.Users.Queries.GetAllUsersByRoleId;

namespace Myrtus.CMS.WebAPI.Controllers.AuditLogs
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/auditlogs")]
    public class AuditLogsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IErrorHandlingService _errorHandlingService;

        public AuditLogsController(ISender sender, IErrorHandlingService errorHandlingService)
        {
            _sender = sender;
            _errorHandlingService = errorHandlingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuditLogs(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = new GetAllAuditLogsQuery(pageIndex, pageSize, cancellationToken);
            Result<IPaginatedList<GetAllAuditLogsQueryResponse>> result = await _sender.Send(query, cancellationToken);

            if (!result.IsSuccess)
            {
                return _errorHandlingService.HandleErrorResponse(result);
            }

            return Ok(result.Value);
        }
    }
}

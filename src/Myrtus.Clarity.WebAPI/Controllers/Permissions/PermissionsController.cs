using Microsoft.AspNetCore.Mvc;
using MediatR;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Application.Features.Permissions.Queries.GetAllPermissions;
using Myrtus.Clarity.WebAPI.Attributes;
using Microsoft.AspNetCore.RateLimiting;
using Myrtus.Clarity.Core.WebAPI;
using Myrtus.Clarity.Core.WebAPI.Controllers;

namespace Myrtus.Clarity.WebAPI.Controllers.PermissionsController
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/permissions")]
    [EnableRateLimiting("fixed")]
    public class PermissionsController(ISender sender, IErrorHandlingService errorHandlingService) : BaseController(sender, errorHandlingService)
    {
        [HttpGet]
        [HasPermission(Permissions.PermissionsRead)]
        public async Task<IActionResult> GetAllPermissions(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            GetAllPermissionsQuery query = new(pageIndex, pageSize);

            Result<IPaginatedList<GetAllPermissionsQueryResponse>> result = await _sender.Send(query, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }
    }
}

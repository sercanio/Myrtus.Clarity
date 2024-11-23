using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Core.WebApi;
using Myrtus.CMS.Application.Features.Permissions.Queries.GetAllPermissions;
using Myrtus.CMS.WebAPI.Attributes;

namespace Myrtus.CMS.WebAPI.Controllers.PermissionsController
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/permissions")]
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

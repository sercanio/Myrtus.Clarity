using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Core.WebApi;
using Myrtus.CMS.Application.Permissions.Queries.GetAllPermissions;

namespace Myrtus.CMS.WebAPI.Controllers.RolePermissions;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/permissions")]
public class PermissionsController : BaseController
{
    public PermissionsController(ISender sender, IErrorHandlingService errorHandlingService)
        : base(sender, errorHandlingService)
    {
    }

    [HttpGet]
    [HasPermission(Permissions.PermissionsRead)]
    public async Task<IActionResult> GetAllPermissions(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllPermissionsQuery(pageIndex, pageSize);

        Result<IPaginatedList<GroupedPermissionsResponse>> result = await _sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return Ok(result.Value);
    }
}

using Ardalis.Result;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Myrtus.Clarity.Application.Features.Notifications.Queries.GetAllNotifications;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Core.WebApi;
using Myrtus.Clarity.WebAPI.Attributes;

namespace Myrtus.Clarity.WebAPI.Controllers.Notifications
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/notifications")]
    public class NotificationsController(ISender sender, IErrorHandlingService errorHandlingService) : BaseController(sender, errorHandlingService)
    {
        [HttpGet]
        [HasPermission(Permissions.NotificationsRead)]
        public async Task<IActionResult> GetAllNotifications(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            GetAllNotificationsQuery query = new(pageIndex, pageSize, cancellationToken);
            Result<IPaginatedList<GetAllNotificationsQueryResponse>> result = await _sender.Send(query, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }
    }
}

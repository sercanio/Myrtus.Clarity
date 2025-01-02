using Ardalis.Result;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Myrtus.Clarity.Application.Features.Notifications.Commands.MarkNotificationsAsRead;
using Myrtus.Clarity.Application.Features.Notifications.Queries.GetAllNotifications;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Core.WebAPI;
using Myrtus.Clarity.Core.WebAPI.Controllers;
using Myrtus.Clarity.WebAPI.Attributes;
using ApiVersion = Microsoft.AspNetCore.Mvc.ApiVersion;
using ApiVersionAttribute = Microsoft.AspNetCore.Mvc.ApiVersionAttribute;

namespace Myrtus.Clarity.WebAPI.Controllers.Notifications
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/notifications")]
    [EnableRateLimiting("fixed")]
    public class NotificationsController(
        ISender sender,
        IErrorHandlingService errorHandlingService) : BaseController(sender, errorHandlingService)
    {
        [HttpGet]
        [HasPermission(Permissions.NotificationsRead)]
        public async Task<IActionResult> GetAllNotifications(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            GetAllNotificationsQuery query = new(pageIndex, pageSize, cancellationToken);
            Result<GetAllNotificationsWithUnreadCountResponse> result = await _sender.Send(query, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }

        [HttpPatch("read")]
        [HasPermission(Permissions.NotificationsUpdate)]
        public async Task<IActionResult> MarkNotificationsAsRead(
            CancellationToken cancellationToken = default)
        {
            MarkNotificationsAsReadCommand command = new(cancellationToken);

            Result<MarkNotificationsAsReadCommandResponse> result = await _sender.Send(command, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok();
        }
    }
}

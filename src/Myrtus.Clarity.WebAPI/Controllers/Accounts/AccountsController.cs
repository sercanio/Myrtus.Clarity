using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;
using Ardalis.Result;
using Myrtus.Clarity.Core.WebApi;
using Myrtus.Clarity.Application.Features.Accounts.LogInUser;
using Myrtus.Clarity.Application.Features.Accounts.RegisterUser;
using Myrtus.Clarity.Application.Features.Users.Queries.GetLoggedInUser;
using Myrtus.Clarity.Domain.Users.ValueObjects;
using Myrtus.Clarity.Application.Features.Accounts.UpdateNotificationPreferences;

namespace Myrtus.Clarity.WebAPI.Controllers.Accounts
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/accounts")]
    public class AccountsController(ISender sender, IErrorHandlingService errorHandlingService) : BaseController(sender, errorHandlingService)
    {
        [HttpGet("me")]
        //[HasPermission(Permissions.UsersRead)]
        public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
        {
            GetLoggedInUserQuery query = new();
            Result<UserResponse> result = await _sender.Send(query, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }

        [HttpPatch("me/notifications")]
        //[HasPermission(Permissions.UsersUpdate)]
        public async Task<IActionResult> UpdateNotifications(
            UpdateUserNotificationsRequest request,
            CancellationToken cancellationToken)
        {
            NotificationPreference notificationPreference = new(
                request.InAppNotification,
                request.EmailNotification,
                request.PushNotification);

            UpdateNotificationPreferencesCommand command = new(notificationPreference);
            Result<UpdateNotificationPreferencesCommandResponse> result = await _sender.Send(command, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : NoContent();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterUserRequest request,
            CancellationToken cancellationToken)
        {
            RegisterUserCommand command = new(
                request.Email,
                request.FirstName,
                request.LastName,
                request.Password);

            Result<Guid> result = await _sender.Send(command, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LogIn(
            LogInUserRequest request,
            CancellationToken cancellationToken)
        {
            LogInUserCommand command = new(request.Email, request.Password);
            Result<AccessTokenResponse> result = await _sender.Send(command, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }
    }
}

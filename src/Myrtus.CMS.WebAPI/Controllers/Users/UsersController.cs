using Asp.Versioning;
using Myrtus.CMS.Application.Users.GetLoggedInUser;
using Myrtus.CMS.Application.Users.LogInUser;
using Myrtus.CMS.Application.Users.RegisterUser;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ardalis.Result;
using Myrtus.Clarity.Core.WebApi;

namespace Myrtus.CMS.WebAPI.Controllers.Users;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/users")]
public class UsersController : BaseController
{
    public UsersController(ISender sender, IErrorHandlingService errorHandlingService)
        : base(sender, errorHandlingService)
    {
    }

    [HttpGet("me")]
    [HasPermission(Permissions.UsersRead)]
    public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();
        Result<UserResponse> result = await _sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LogIn(
        LogInUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(request.Email, request.Password);
        Result<AccessTokenResponse> result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return Ok(result.Value);
    }
}

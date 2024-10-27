using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;
using Ardalis.Result;
using Myrtus.Clarity.Core.WebApi;
using Myrtus.CMS.Application.Users.GiveRoleToUser;
using Myrtus.CMS.Application.Users.TakeRoleFromUser;
using Myrtus.CMS.Application.Users.GetAllUsers;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.CMS.Application.Users.GetUser;
using Myrtus.CMS.Application.Users.Update.UpdateUserRoles;

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

    [HttpGet]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] GetAllUsersRequest request,
        CancellationToken cancellationToken
        )
    {
        var query = new GetAllUsersQuery(request.Pagination.PageIndex, request.Pagination.PageSize);

        Result<IPaginatedList<GetUserQueryResponse>> result = await _sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return Ok(result.Value);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUserQuery(userId);

        Result<GetUserQueryResponse> result = await _sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return Ok(result.Value);
    }


    [HttpPatch("{userId}/roles")]
    public async Task<IActionResult> UpdateUserRoles(
        UpdateUserRolesRequest request,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateUserRolesCommand(userId, request.Operation, request.RoleId);

        Result<UpdateUserRolesCommandResponse> result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return Ok(result.Value);
    }

}
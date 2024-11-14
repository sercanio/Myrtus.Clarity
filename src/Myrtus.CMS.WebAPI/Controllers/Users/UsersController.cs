using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.WebApi;
using Myrtus.CMS.Application.Users.Queries.GetAllUsers;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.CMS.Application.Users.Queries.GetUser;
using Myrtus.CMS.Application.Users.Commands.Update.UpdateUserRoles;
using Myrtus.CMS.Application.Users.Queries.GetAllUsersDynamic;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;

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
    [HasPermission(Permissions.UsersRead)]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllUsersQuery(pageIndex, pageSize);

        Result<IPaginatedList<GetAllUsersQueryResponse>> result = await _sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return Ok(result.Value);
    }

    [HttpPost("dynamic")]
    [HasPermission(Permissions.UsersRead)]
    public async Task<IActionResult> GetAllUsersDynamic(
    [FromBody] DynamicQuery dynamicQuery,
    [FromQuery] int pageIndex = 0,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var query = new GetAllUsersDynamicQuery(
            pageIndex,
            pageSize,
            dynamicQuery
        );

        Result<IPaginatedList<GetAllUsersDynamicQueryResponse>> result = await _sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return Ok(result.Value);
    }

    [HttpGet("{userId}")]
    [HasPermission(Permissions.UsersRead)]
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
    [HasPermission(Permissions.UsersRead)]
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
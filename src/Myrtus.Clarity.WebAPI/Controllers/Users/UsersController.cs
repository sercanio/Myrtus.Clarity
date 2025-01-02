using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Core.Infrastructure.Dynamic;
using Myrtus.Clarity.Application.Features.Users.Queries.GetAllUsers;
using Myrtus.Clarity.Application.Features.Users.Queries.GetAllUsersByRoleId;
using Myrtus.Clarity.Application.Features.Users.Queries.GetAllUsersDynamic;
using Myrtus.Clarity.Application.Features.Users.Queries.GetUser;
using Myrtus.Clarity.Application.Features.Users.Commands.Update.UpdateUserRoles;
using Myrtus.Clarity.WebAPI.Attributes;
using Microsoft.AspNetCore.RateLimiting;
using Myrtus.Clarity.Core.WebAPI;
using Myrtus.Clarity.Core.WebAPI.Controllers;
using ApiVersion = Microsoft.AspNetCore.Mvc.ApiVersion;
using ApiVersionAttribute = Microsoft.AspNetCore.Mvc.ApiVersionAttribute;

namespace Myrtus.Clarity.WebAPI.Controllers.Users
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/users")]
    [EnableRateLimiting("fixed")]
    public class UsersController(ISender sender, IErrorHandlingService errorHandlingService) : BaseController(sender, errorHandlingService)
    {
        [HttpGet]
        [HasPermission(Permissions.UsersRead)]
        public async Task<IActionResult> GetAllUsers(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            GetAllUsersQuery query = new(pageIndex, pageSize);

            Result<IPaginatedList<GetAllUsersQueryResponse>> result = await _sender.Send(query, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }

        [HttpGet("roles/{roleId}")]
        [HasPermission(Permissions.UsersRead)]
        public async Task<IActionResult> GetAllUsersByRoleId(
            Guid roleId,
            [FromQuery] int PageIndex = 0,
            [FromQuery] int PageSize = 10,
            CancellationToken cancellationToken = default)
        {
            GetAllUsersByRoleIdQuery query = new(PageIndex, PageSize, roleId);

            Result<IPaginatedList<GetAllUsersByRoleIdQueryResponse>> result = await _sender.Send(query, cancellationToken);
            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }

        [HttpPost("dynamic")]
        [HasPermission(Permissions.UsersRead)]
        public async Task<IActionResult> GetAllUsersDynamic(
            [FromBody] DynamicQuery dynamicQuery,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        {
            GetAllUsersDynamicQuery query = new(
                pageIndex,
                pageSize,
                dynamicQuery
            );

            Result<IPaginatedList<GetAllUsersDynamicQueryResponse>> result = await _sender.Send(query, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }

        [HttpGet("{userId}")]
        [HasPermission(Permissions.UsersRead)]
        public async Task<IActionResult> GetUserById(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            GetUserQuery query = new(userId);

            Result<GetUserQueryResponse> result = await _sender.Send(query, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }


        [HttpPatch("{userId}/roles")]
        [HasPermission(Permissions.UsersRead)]
        public async Task<IActionResult> UpdateUserRoles(
            UpdateUserRolesRequest request,
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            UpdateUserRolesCommand command = new(userId, request.Operation, request.RoleId);

            Result<UpdateUserRolesCommandResponse> result = await _sender.Send(command, cancellationToken);

            return !result.IsSuccess ? _errorHandlingService.HandleErrorResponse(result) : Ok(result.Value);
        }
    }
}
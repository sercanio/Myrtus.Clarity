﻿using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Core.WebApi;
using Myrtus.CMS.Application.Features.Roles.Commands.Create;
using Myrtus.CMS.Application.Features.Roles.Commands.Delete;
using Myrtus.CMS.Application.Features.Roles.Queries.GetAllRoles;
using Myrtus.CMS.Application.Features.Roles.Queries.GetRoleById;
using Myrtus.CMS.Application.Features.Roles.Commands.Update.UpdatePermissions;
using Myrtus.CMS.Application.Features.Roles.Commands.Update.UpdateRoleName;

namespace Myrtus.CMS.WebAPI.Controllers.UserRoles;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/roles")]
public class RolesController : BaseController
{
    public RolesController(ISender sender, IErrorHandlingService errorHandlingService)
        : base(sender, errorHandlingService)
    {
    }

    [HttpGet]
    [HasPermission(Permissions.RolesRead)]
    public async Task<IActionResult> GetAllRoles(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllRolesQuery(pageIndex, pageSize);
        Result<IPaginatedList<GetAllRolesQueryResponse>> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpPost]
    [HasPermission(Permissions.RolesCreate)]
    public async Task<IActionResult> CreateRole(
        CreateRoleRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateRoleCommand(request.Name);
        Result<CreateRoleCommandResponse> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpGet("{roleId}")]
    [HasPermission(Permissions.RolesRead)]
    public async Task<IActionResult> GetRoleById([FromRoute] Guid roleId, CancellationToken cancellationToken = default)
    {
        var query = new GetRoleByIdQuery(roleId);
        Result<GetRoleByIdQueryResponse> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpPatch("{roleId}/permissions")]
    [HasPermission(Permissions.RolesUpdate)]
    public async Task<IActionResult> UpdateRolePermissions(
        [FromBody] UpdateRolePermissionsRequest request,
        [FromRoute] Guid roleId,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateRolePermissionsCommand(roleId, request.PermissionId, request.Operation);
        Result<UpdateRolePermissionsCommandResponse> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpPatch("{roleId}/name")]
    [HasPermission(Permissions.RolesUpdate)]
    public async Task<IActionResult> UpdateRoleName(
        [FromBody] UpdateRoleNameRequest request,
        [FromRoute] Guid roleId,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateRoleNameCommand(roleId, request.Name);
        Result<UpdateRoleNameCommandResponse> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }

    [HttpDelete("{roleId}")]
    [HasPermission(Permissions.RolesDelete)]
    public async Task<IActionResult> DeleteRole(
        [FromRoute] Guid roleId,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteRoleCommand(roleId);
        Result<DeleteRoleCommandResponse> result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
    }
}

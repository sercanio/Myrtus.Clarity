using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;
using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Pagination;
using Myrtus.Clarity.Core.Infrastructure.Authorization;
using Myrtus.Clarity.Core.WebApi;
using Myrtus.Clarity.Application.Features.Roles.Commands.Create;
using Myrtus.Clarity.Application.Features.Roles.Commands.Delete;
using Myrtus.Clarity.Application.Features.Roles.Queries.GetAllRoles;
using Myrtus.Clarity.Application.Features.Roles.Queries.GetRoleById;
using Myrtus.Clarity.Application.Features.Roles.Commands.Update.UpdatePermissions;
using Myrtus.Clarity.Application.Features.Roles.Commands.Update.UpdateRoleName;
using Myrtus.Clarity.WebAPI.Controllers;
using Myrtus.Clarity.WebAPI.Controllers.Roles;

namespace Myrtus.Clarity.WebAPI.Attributes.Roles
{
    [ApiController]
    [ApiVersion(ApiVersions.V1)]
    [Route("api/v{version:apiVersion}/roles")]
    public class RolesController(ISender sender, IErrorHandlingService errorHandlingService) : BaseController(sender, errorHandlingService)
    {
        [HttpGet]
        [HasPermission(Permissions.RolesRead)]
        public async Task<IActionResult> GetAllRoles(
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            GetAllRolesQuery query = new(pageIndex, pageSize);
            Result<IPaginatedList<GetAllRolesQueryResponse>> result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
        }

        [HttpPost]
        [HasPermission(Permissions.RolesCreate)]
        public async Task<IActionResult> CreateRole(
            CreateRoleRequest request,
            CancellationToken cancellationToken = default)
        {
            CreateRoleCommand command = new(request.Name);
            Result<CreateRoleCommandResponse> result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
        }

        [HttpGet("{roleId}")]
        [HasPermission(Permissions.RolesRead)]
        public async Task<IActionResult> GetRoleById([FromRoute] Guid roleId, CancellationToken cancellationToken = default)
        {
            GetRoleByIdQuery query = new(roleId);
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
            UpdateRolePermissionsCommand command = new(roleId, request.PermissionId, request.Operation);
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
            UpdateRoleNameCommand command = new(roleId, request.Name);
            Result<UpdateRoleNameCommandResponse> result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
        }

        [HttpDelete("{roleId}")]
        [HasPermission(Permissions.RolesDelete)]
        public async Task<IActionResult> DeleteRole(
            [FromRoute] Guid roleId,
            CancellationToken cancellationToken = default)
        {
            DeleteRoleCommand command = new(roleId);
            Result<DeleteRoleCommandResponse> result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : _errorHandlingService.HandleErrorResponse(result);
        }
    }
}

using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Commands;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Enums;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Application.Roles.Commands.Update.UpdatePermissions;

namespace Myrtus.CMS.Application.Roles.Commands.Update;

public sealed class UpdateRolePermissionsCommandHandler : BaseCommandHandler<UpdateRolePermissionsCommand, UpdateRolePermissionsCommandResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly IUserContext _userContext;

    public UpdateRolePermissionsCommandHandler(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IPermissionRepository permissionRepository,
        IUserContext userContext,
        IAuditLogService auditLogService,
        IHttpContextAccessor httpContextAccessor)
        : base(auditLogService, httpContextAccessor)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _permissionRepository = permissionRepository;
        _userContext = userContext;
    }

    public override async Task<Result<UpdateRolePermissionsCommandResponse>> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(
            predicate: r => r.Id == request.RoleId,
            include: r => r.Permissions,
            cancellationToken: cancellationToken);

        if (role is null)
            return Result.NotFound();

        var permission = role.Permissions.FirstOrDefault(p => p.Id == request.PermissionId);

        var permissionToAdd = await _permissionRepository.GetAsync(
            predicate: permission => permission.Id == request.PermissionId,
            cancellationToken: cancellationToken);

        if (request.Operation == OperationEnum.Add && permission is null)
        {

            if (permissionToAdd is null)
                return Result.NotFound($"Permission with ID {request.PermissionId} not found.");

            role.Permissions.Add(permissionToAdd);
        }
        else if (request.Operation == OperationEnum.Remove && permission is not null)
        {
            role.Permissions.Remove(permission);
        }
        else
        {
            return Result.Invalid();
        }

        _roleRepository.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"roles-{role.Id}", cancellationToken);
        await _cacheService.RemoveAsync($"auth:roles-{role.Id}", cancellationToken);
        await _cacheService.RemoveAsync($"auth:permissions-{_userContext.IdentityId}", cancellationToken);

        _ = request.Operation switch
        {
            OperationEnum.Add => LogAuditAsync("AddRolePermission", "Role", role.Name, $"Permission '{permissionToAdd.Name}' added to role."),
            OperationEnum.Remove => LogAuditAsync("RemoveRolePermission", "Role", role.Name, $"Permission '{permission.Name}' removed from role."),
            _ => Task.CompletedTask
        };

        return Result.Success(new UpdateRolePermissionsCommandResponse(role.Id, request.PermissionId));
    }
}

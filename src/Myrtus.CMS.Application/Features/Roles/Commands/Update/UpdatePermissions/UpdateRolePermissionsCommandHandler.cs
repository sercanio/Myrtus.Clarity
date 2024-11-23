using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Enums;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Application.Services.Users;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Update.UpdatePermissions;

public sealed class UpdateRolePermissionsCommandHandler : ICommandHandler<UpdateRolePermissionsCommand, UpdateRolePermissionsCommandResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly IUserContext _userContext;

    public UpdateRolePermissionsCommandHandler(
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        IUserService userRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IUserContext userContext)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _userService = userRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _userContext = userContext;
    }

    public async Task<Result<UpdateRolePermissionsCommandResponse>> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
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

            role.AddPermission(permissionToAdd);
        }
        else if (request.Operation == OperationEnum.Remove && permission is not null)
        {
            role.RemovePermission(permission);
        }
        else
        {
            return Result.Invalid();
        }

        var user = await _userService.GetUserByIdAsync(_userContext.UserId, cancellationToken);
        role.UpdatedBy = user!.Email;

        _roleRepository.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"roles-{role.Id}", cancellationToken);
        await _cacheService.RemoveAsync($"auth:roles-{role.Id}", cancellationToken);
        await _cacheService.RemoveAsync($"auth:permissions-{_userContext.IdentityId}", cancellationToken);

        return Result.Success(new UpdateRolePermissionsCommandResponse(role.Id, request.PermissionId));
    }
}

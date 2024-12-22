using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Application.Enums;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Application.Services.Users;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Users;
using Myrtus.Clarity.Core.Infrastructure.Pagination;
using System.Data;

namespace Myrtus.Clarity.Application.Features.Roles.Commands.Update.UpdatePermissions
{
    public sealed class UpdateRolePermissionsCommandHandler(
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        IUserService userService,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IUserContext userContext) : ICommandHandler<UpdateRolePermissionsCommand, UpdateRolePermissionsCommandResponse>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IPermissionRepository _permissionRepository = permissionRepository;
        private readonly IUserService _userService = userService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IUserContext _userContext = userContext;

        public async Task<Result<UpdateRolePermissionsCommandResponse>> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            Role? role = await _roleRepository.GetAsync(
                predicate: r => r.Id == request.RoleId,
                include: r => r.Permissions,
                cancellationToken: cancellationToken);

            if (role is null)
            {
                return Result.NotFound();
            }

            Permission? permission = role.Permissions.FirstOrDefault(p => p.Id == request.PermissionId);

            Permission? permissionToAdd = await _permissionRepository.GetAsync(
                predicate: permission => permission.Id == request.PermissionId,
                cancellationToken: cancellationToken);

            if (request.Operation == Operation.Add && permission is null)
            {

                if (permissionToAdd is null)
                {
                    return Result.NotFound($"Permission with ID {request.PermissionId} not found.");
                }

                role.AddPermission(permissionToAdd);
            }
            else if (request.Operation == Operation.Remove && permission is not null)
            {
                role.RemovePermission(permission);
            }
            else
            {
                return Result.Invalid();
            }

            User user = await _userService.GetUserByIdAsync(_userContext.UserId, cancellationToken);
            role.UpdatedBy = user!.Email.Value;

            _roleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await InvalidateCachesAsync(role.Id, cancellationToken);

            return Result.Success(new UpdateRolePermissionsCommandResponse(role.Id, request.PermissionId));
        }

        private async Task InvalidateCachesAsync(Guid roleId, CancellationToken cancellationToken)
        {
            await _cacheService.RemoveAsync($"roles-{roleId}", cancellationToken);

            const int batchSize = 1000;
            int pageIndex = 0;
            PaginatedList<User> usersBatch;

            do
            {
                usersBatch = await _userService.GetAllAsync(
                    index: pageIndex,
                    size: batchSize,
                    includeSoftDeleted: false,
                    predicate: u => u.Roles.Any(r => r.Id == roleId),
                    cancellationToken);

                var tasks = usersBatch.Items.Select(async u =>
                {
                    await _cacheService.RemoveAsync($"auth:roles-{u.IdentityId}", cancellationToken);
                    await _cacheService.RemoveAsync($"auth:permissions-{u.IdentityId}", cancellationToken);
                });

                await Task.WhenAll(tasks);

                pageIndex++;
            } while (usersBatch.Items.Count > 0);
        }
    }
}

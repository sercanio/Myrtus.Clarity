using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Application.Services.Users;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Users;
using Myrtus.Clarity.Core.Infrastructure.Pagination;

namespace Myrtus.Clarity.Application.Features.Roles.Commands.Update.UpdateRoleName
{
    public sealed class UpdateRoleNameCommandHandler(
           IRoleRepository roleRepository,
           IUserService userRepository,
           IUserContext userContext,
           IUnitOfWork unitOfWork,
           ICacheService cacheService) : ICommandHandler<UpdateRoleNameCommand, UpdateRoleNameCommandResponse>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IUserService _userService = userRepository;
        private readonly IUserContext _userContext = userContext;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Result<UpdateRoleNameCommandResponse>> Handle(UpdateRoleNameCommand request, CancellationToken cancellationToken)
        {
            Role? role = await _roleRepository.GetAsync(
                predicate: r => r.Id == request.RoleId,
                include: r => r.Permissions,
                cancellationToken: cancellationToken);

            if (role is null)
            {
                return Result.NotFound();
            }

            role.ChangeName(request.Name);

            User user = await _userService.GetUserByIdAsync(_userContext.UserId, cancellationToken);
            role.UpdatedBy = user.Email;

            _roleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await InvalidateRoleCacheAsync(role.Id, cancellationToken);

            return Result.Success(new UpdateRoleNameCommandResponse(role.Name));
        }

        private async Task InvalidateRoleCacheAsync(Guid roleId, CancellationToken cancellationToken)
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

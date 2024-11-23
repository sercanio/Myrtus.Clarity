using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Enums;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Application.Services.Roles;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Features.Users.Commands.Update.UpdateUserRoles
{
    public sealed class UpdateUserRolesCommandHandler(
        IUserRepository userRepository,
        IRoleService roleService,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IUserContext userContext) : ICommandHandler<UpdateUserRolesCommand, UpdateUserRolesCommandResponse>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRoleService _roleService = roleService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IUserContext _userContext = userContext;

        public async Task<Result<UpdateUserRolesCommandResponse>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetAsync(
                predicate: user => user.Id == request.UserId,
                include: user => user.Roles,
                cancellationToken: cancellationToken);

            if (user is null)
            {
                return Result.NotFound(UserErrors.NotFound.Name);
            }

            Role? role = await _roleService.GetAsync(
                predicate: role => role.Id == request.RoleId,
                cancellationToken: cancellationToken);

            if (role is null)
            {
                return Result.NotFound(RoleErrors.NotFound.Name);
            }

            switch (request.Operation)
            {
                case Operation.Add:
                    user.AddRole(role);
                    break;
                case Operation.Remove:
                    user.RemoveRole(role);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            User? modifierUser = await _userRepository.GetUserByIdAsync(_userContext.UserId, cancellationToken);
            user.UpdatedBy = modifierUser!.Email ?? "System";

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"users-{user.Id}", cancellationToken);
            await _cacheService.RemoveAsync($"auth:roles-{_userContext.IdentityId}", cancellationToken);
            await _cacheService.RemoveAsync($"auth:permissions-{_userContext.IdentityId}", cancellationToken);

            UpdateUserRolesCommandResponse response = new(role.Id, user.Id);
            return Result.Success(response);
        }
    }
}

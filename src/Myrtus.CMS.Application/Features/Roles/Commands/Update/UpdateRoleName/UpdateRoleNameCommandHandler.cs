using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Application.Services.Users;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Users;

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

            await _cacheService.RemoveAsync($"roles-{role.Id}", cancellationToken);
            await _cacheService.RemoveAsync($"auth:roles-{role.Id}", cancellationToken);

            return Result.Success(new UpdateRoleNameCommandResponse(role.Name));
        }
    }
}

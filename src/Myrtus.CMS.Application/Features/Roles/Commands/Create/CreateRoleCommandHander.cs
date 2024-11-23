using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Application.Services.Users;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Users;

namespace Myrtus.Clarity.Application.Features.Roles.Commands.Create
{
    public sealed class CreateRoleCommandHander(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IUserService userRepository,
        IUserContext userContext) : ICommandHandler<CreateRoleCommand, CreateRoleCommandResponse>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;
        private readonly IUserService _userService = userRepository;
        private readonly IUserContext _userContext = userContext;

        public async Task<Result<CreateRoleCommandResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {

            bool nameExists = await _roleRepository.ExistsAsync(
                    predicate: role => role.Name == request.Name,
                    cancellationToken: cancellationToken);

            if (nameExists)
            {
                return Result.Conflict(RoleErrors.Overlap.Name);
            }

            Role role = Role.Create(request.Name);

            User? user = await _userService.GetUserByIdAsync(_userContext.UserId,
                cancellationToken: cancellationToken);
            if (user is not null)
            {
                role.CreatedBy = user.Email;
            }

            await _roleRepository.AddAsync(role);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _cacheService.RemoveAsync($"roles-{role.Id}", cancellationToken);

            CreateRoleCommandResponse response = new(
                role.Id,
                role.Name);

            return Result.Success(response);
        }
    }
}
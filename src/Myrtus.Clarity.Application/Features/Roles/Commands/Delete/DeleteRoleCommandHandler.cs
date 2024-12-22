using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Application.Services.Users;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Users;

namespace Myrtus.Clarity.Application.Features.Roles.Commands.Delete
{
    public sealed class DeleteRoleCommandHandler(
        IRoleRepository roleRepository,
        IUserService userRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        ICacheService cacheService) : ICommandHandler<DeleteRoleCommand, DeleteRoleCommandResponse>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IUserService _userService = userRepository;
        private readonly IUserContext _userContext = userContext;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Result<DeleteRoleCommandResponse>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            Role? role = await _roleRepository.GetAsync(
                predicate: role => role.Id == request.RoleId,
                cancellationToken: cancellationToken);

            if (role is null)
            {
                return Result.NotFound(RoleErrors.NotFound.Name);
            }

            User? user = await _userService.GetUserByIdAsync(
                _userContext.UserId,
                cancellationToken: cancellationToken);
            if (user is not null)
            {
                role.UpdatedBy = user.Email.Value;
            }

            try
            {
                _ = Role.Delete(role);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _cacheService.RemoveAsync($"roles-{role.Id}", cancellationToken);

                DeleteRoleCommandResponse response = new(role.Id, role.Name);
                return Result.Success(response);
            }
            catch (InvalidOperationException ex)
            {
                return Result.Forbidden(ex.Message);
            }
        }
    }
}

using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Enums;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Features.Users.Commands.Update.UpdateUserRoles;

public sealed class UpdateUserRolesCommandHandler : ICommandHandler<UpdateUserRolesCommand, UpdateUserRolesCommandResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly IUserContext _userContext;

    public UpdateUserRolesCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IUserContext userContext)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _userContext = userContext;
    }

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

        Role? role = await _roleRepository.GetAsync(
            predicate: role => role.Id == request.RoleId,
            cancellationToken: cancellationToken);

        if (role is null)
        {
            return Result.NotFound(RoleErrors.NotFound.Name);
        }

        switch (request.Operation)
        {
            case OperationEnum.Add:
                user.AddRole(role);
                break;
            case OperationEnum.Remove:
                user.RemoveRole(role);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        User? modifierUser = await _userRepository.GetUserByIdAsync(_userContext.UserId, cancellationToken);
        user.UpdatedBy = modifierUser!.Email ?? "System";

        _userRepository.Update(user);
        _ = await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"users-{user.Id}", cancellationToken);
        await _cacheService.RemoveAsync($"auth:roles-{_userContext.IdentityId}", cancellationToken);
        await _cacheService.RemoveAsync($"auth:permissions-{_userContext.IdentityId}", cancellationToken);

        UpdateUserRolesCommandResponse response = new(role.Id, user.Id);
        return Result.Success(response);
    }
}

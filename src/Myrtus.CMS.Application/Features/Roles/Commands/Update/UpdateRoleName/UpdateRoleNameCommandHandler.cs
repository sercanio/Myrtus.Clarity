using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Application.Services.Users;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Update.UpdateRoleName;

public sealed class UpdateRoleNameCommandHandler : ICommandHandler<UpdateRoleNameCommand, UpdateRoleNameCommandResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserService _userService;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public UpdateRoleNameCommandHandler(
       IRoleRepository roleRepository,
       IUserService userRepository,
       IUserContext userContext,
       IUnitOfWork unitOfWork,
       ICacheService cacheService)
    {
        _roleRepository = roleRepository;
        _userService = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<UpdateRoleNameCommandResponse>> Handle(UpdateRoleNameCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(
            predicate: r => r.Id == request.RoleId,
            include: r => r.Permissions,
            cancellationToken: cancellationToken);

        if (role is null)
            return Result.NotFound();

        role.ChangeName(request.Name);

        var user = await _userService.GetUserByIdAsync(_userContext.UserId, cancellationToken);
        role.UpdatedBy = user.Email;

        _roleRepository.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"roles-{role.Id}", cancellationToken);
        await _cacheService.RemoveAsync($"auth:roles-{role.Id}", cancellationToken);

        return Result.Success(new UpdateRoleNameCommandResponse(role.Name));
    }
}

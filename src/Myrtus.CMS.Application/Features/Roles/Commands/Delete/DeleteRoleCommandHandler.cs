using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Delete;

public sealed class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, DeleteRoleCommandResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public DeleteRoleCommandHandler(
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<DeleteRoleCommandResponse>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(
            predicate: role => role.Id == request.RoleId,
            cancellationToken: cancellationToken);

        if (role is null)
        {
            return Result.NotFound(RoleErrors.NotFound.Name);
        }

        User? user = await _userRepository.GetUserByIdAsync(
            _userContext.UserId,
            cancellationToken: cancellationToken);
        if (user is not null)
        {
            role.UpdatedBy = user.Email;
        }

        try
        {
            _ = Role.Delete(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _cacheService.RemoveAsync($"roles-{role.Id}", cancellationToken);

            DeleteRoleCommandResponse response = new DeleteRoleCommandResponse(role.Id, role.Name);
            return Result.Success(response);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Forbidden(ex.Message);
        }
    }
}

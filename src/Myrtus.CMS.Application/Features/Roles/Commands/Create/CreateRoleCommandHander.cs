using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Authentication.Keycloak;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Create;

public sealed class CreateRoleCommandHander : ICommandHandler<CreateRoleCommand, CreateRoleCommandResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly IUserRepository _userRepository;
    private readonly IUserContext _userContext;

    public CreateRoleCommandHander(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IUserRepository userRepository,
        IUserContext userContext)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _userRepository = userRepository;
        _userContext = userContext;
    }

    public async Task<Result<CreateRoleCommandResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {

        bool nameExists = await _roleRepository.ExistsAsync(
                predicate: role => role.Name == request.Name,
                cancellationToken: cancellationToken);

        if (nameExists)
        {
            return Result.Conflict(RoleErrors.Overlap.Name);
        }

        var role = Role.Create(request.Name);

        User? user = await _userRepository.GetUserByIdAsync(_userContext.UserId,
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
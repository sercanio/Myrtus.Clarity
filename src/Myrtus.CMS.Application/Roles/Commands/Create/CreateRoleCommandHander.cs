using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Application.Roles.Commands.Create;

public sealed class CreateRoleCommandHander : ICommandHandler<CreateRoleCommand, CreateRoleCommandResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public CreateRoleCommandHander(IRoleRepository roleRepository, IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
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

        await _roleRepository.AddAsync(role);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        CreateRoleCommandResponse response = new CreateRoleCommandResponse(
            role.Id,
            role.Name);

        return Result.Success(response);
    }
}

using Ardalis.Result;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Application.Roles.Commands.Update;

public sealed class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand, UpdateRoleCommandResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;


    public UpdateRoleCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork, ICacheService cacheService)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Result<UpdateRoleCommandResponse>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(
            predicate: role => role.Id == request.RoleId,
            include: role => role.Permissions,
            cancellationToken: cancellationToken);

        if (role == null)
        {
            return Result.NotFound();
        }

        role.Permissions = new List<Permission>();

        foreach(Permission permission in request.Permissions)
        {
            role.Permissions.Add(permission);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _cacheService.RemoveAsync($"roles-{role.Id}", cancellationToken);

        UpdateRoleCommandResponse response = new UpdateRoleCommandResponse(
            role.Id,
            role.Permissions);

        return Result.Success(response);
    }
}

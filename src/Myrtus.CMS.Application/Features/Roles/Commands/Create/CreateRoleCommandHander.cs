using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Commands;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Create;

public sealed class CreateRoleCommandHander : BaseCommandHandler<CreateRoleCommand, CreateRoleCommandResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public CreateRoleCommandHander(
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService,
        IAuditLogService auditLogService,
        IHttpContextAccessor httpContextAccessor
        ) : base(auditLogService, httpContextAccessor)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public override async Task<Result<CreateRoleCommandResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
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

        await LogAuditAsync("CreateRole", "Role", role.Name, $"Role '{role.Name}' created.");

        CreateRoleCommandResponse response = new CreateRoleCommandResponse(
            role.Id,
            role.Name);

        return Result.Success(response);
    }
}
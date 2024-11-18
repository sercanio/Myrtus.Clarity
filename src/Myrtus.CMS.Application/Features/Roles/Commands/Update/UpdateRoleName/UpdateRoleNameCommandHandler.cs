using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Commands;
using Myrtus.Clarity.Core.Application.Abstractions.Messaging;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;
using System.Security.Claims;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Update.UpdateRoleName;

public sealed class UpdateRoleNameCommandHandler : BaseCommandHandler<UpdateRoleNameCommand, UpdateRoleNameCommandResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public UpdateRoleNameCommandHandler(
       IRoleRepository roleRepository,
       IUnitOfWork unitOfWork,
       ICacheService cacheService,
       IAuditLogService auditLogService,
       IHttpContextAccessor httpContextAccessor)
       : base(auditLogService, httpContextAccessor)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public override async Task<Result<UpdateRoleNameCommandResponse>> Handle(UpdateRoleNameCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(
            predicate: r => r.Id == request.RoleId,
            include: r => r.Permissions,
            cancellationToken: cancellationToken);

        if (role is null)
            return Result.NotFound();

        var oldName = role.Name;
        role = Role.ChangeName(role, request.Name);

        _roleRepository.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cacheService.RemoveAsync($"roles-{role.Id}", cancellationToken);
        await _cacheService.RemoveAsync($"auth:roles-{role.Id}", cancellationToken);

        await LogAuditAsync("UpdateRoleName", "Role", role.Name, $"Role name changed from '{oldName}' to '{role.Name}'");

        return Result.Success(new UpdateRoleNameCommandResponse(role.Name));
    }
}

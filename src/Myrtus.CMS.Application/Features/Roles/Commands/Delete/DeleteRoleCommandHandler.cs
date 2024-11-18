using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using Myrtus.Clarity.Core.Application.Abstractions.Commands;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Delete;

public sealed class DeleteRoleCommandHandler : BaseCommandHandler<DeleteRoleCommand, DeleteRoleCommandResponse>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public DeleteRoleCommandHandler(
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

    public override async Task<Result<DeleteRoleCommandResponse>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetAsync(
            predicate: role => role.Id == request.RoleId,
            cancellationToken: cancellationToken);

        if (role is null)
        {
            return Result.NotFound(RoleErrors.NotFound.Name);
        }

        try
        {
            _roleRepository.Delete(role);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _cacheService.RemoveAsync($"roles-{role.Id}", cancellationToken);

            await LogAuditAsync("DeleteRole", "Role", role.Name, $"Role '{role.Name}' deleted.");

            DeleteRoleCommandResponse response = new DeleteRoleCommandResponse(role.Id, role.Name);
            return Result.Success(response);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Forbidden(ex.Message);
        }
    }
}

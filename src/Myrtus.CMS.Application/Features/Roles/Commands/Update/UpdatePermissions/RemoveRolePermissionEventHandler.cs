using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Roles.Events;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Update.UpdatePermissions;

internal class RemoveRolePermissionEventHandler : INotificationHandler<RolePermissionRemovedDomainEvent>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IAuditLogService _auditLogService;

    public RemoveRolePermissionEventHandler(
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        IAuditLogService auditLogService)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _auditLogService = auditLogService;
    }

    public async Task Handle(RolePermissionRemovedDomainEvent notification, CancellationToken cancellationToken)
    {
        Role? role = await _roleRepository.GetAsync(
            predicate: role => role.Id == notification.RoleId,
            cancellationToken: cancellationToken);

        Permission? permission = await _permissionRepository.GetAsync(
            predicate: permission => permission.Id == notification.PermissionId,
            cancellationToken: cancellationToken);

        AuditLog log = new()
        {
            User = role!.UpdatedBy!,
            Action = RoleDomainEvents.RemovedPermission,
            Entity = role.GetType().Name,
            EntityId = role.Id.ToString(),
            Details = $"{role.GetType().Name} '{role.Name}' has been revoked permission '{permission!.Name}'."
        };
        await _auditLogService.LogAsync(log);
    }
}

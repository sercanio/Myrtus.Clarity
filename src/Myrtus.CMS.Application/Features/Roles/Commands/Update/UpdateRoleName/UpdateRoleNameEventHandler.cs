using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Roles.Events;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Update.UpdateRoleName;

internal class UpdateRoleNameEventHandler : INotificationHandler<RoleNameUpdatedDomainEvent>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IAuditLogService _auditLogService;

    public UpdateRoleNameEventHandler(
        IRoleRepository roleRepository,
        IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
        _roleRepository = roleRepository;
    }

    public async Task Handle(RoleNameUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Role? role = await _roleRepository.GetAsync(
            predicate: role => role.Id == notification.RoleId,
            cancellationToken: cancellationToken);

        string oldName = notification.OldRoleName;

        AuditLog log = new()
        {
            User = role!.UpdatedBy!,
            Action = RoleDomainEvents.UpdatedName,
            Entity = role.GetType().Name,
            EntityId = role.Id.ToString(),
            Details = $"{role.GetType().Name} '{oldName}' has been updated to '{role.Name}'."
        };
        await _auditLogService.LogAsync(log);
    }
}

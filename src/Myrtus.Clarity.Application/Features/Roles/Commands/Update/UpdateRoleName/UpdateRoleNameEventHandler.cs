using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Roles.Events;

namespace Myrtus.Clarity.Application.Features.Roles.Commands.Update.UpdateRoleName
{
    internal class UpdateRoleNameEventHandler(
        IRoleRepository roleRepository,
        IAuditLogService auditLogService) : INotificationHandler<RoleNameUpdatedDomainEvent>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IAuditLogService _auditLogService = auditLogService;

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
}

using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Roles.Events;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Create
{
    internal class CreateRoleEventHandler(
        IRoleRepository roleRepository,
        IAuditLogService auditLogService) : INotificationHandler<RoleCreatedDomainEvent>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IAuditLogService _auditLogService = auditLogService;

        public async Task Handle(RoleCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            Role? role = await _roleRepository.GetAsync(
                predicate: role => role.Id == notification.RoleId,
                cancellationToken: cancellationToken);

            AuditLog log = new()
            {
                User = role!.CreatedBy,
                Action = RoleDomainEvents.Created,
                Entity = role.GetType().Name,
                EntityId = role.Id.ToString(),
                Details = $"{role.GetType().Name} '{role.Name}' has been created."
            };
            await _auditLogService.LogAsync(log);
        }
    }
}

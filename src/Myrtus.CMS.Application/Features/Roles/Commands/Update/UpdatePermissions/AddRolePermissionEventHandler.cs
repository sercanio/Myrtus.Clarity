﻿using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Roles;
using Myrtus.CMS.Domain.Roles.Events;

namespace Myrtus.CMS.Application.Features.Roles.Commands.Update.UpdatePermissions
{
    internal class AddRolePermissionEventHandler(
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        IAuditLogService auditLogService) : INotificationHandler<RolePermissionAddedDomainEvent>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IPermissionRepository _permissionRepository = permissionRepository;
        private readonly IAuditLogService _auditLogService = auditLogService;

        public async Task Handle(RolePermissionAddedDomainEvent notification, CancellationToken cancellationToken)
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
                Action = RoleDomainEvents.AddedPermission,
                Entity = role.GetType().Name,
                EntityId = role.Id.ToString(),
                Details = $"{role.GetType().Name} '{role.Name}' has been granted permission '{permission!.Name}'."
            };
            await _auditLogService.LogAsync(log);
        }
    }

}

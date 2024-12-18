using MediatR;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Application.Abstractions.Mailing;
using Myrtus.Clarity.Core.Application.Abstractions.Notification;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Core.Domain.Abstractions.Mailing;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Roles.Events;
using System.Text;

namespace Myrtus.Clarity.Application.Features.Roles.Commands.Create
{
    internal class CreateRoleEventHandler(
        IRoleRepository roleRepository,
        IMailService emailService,
        INotificationService notificationService,
        IAuditLogService auditLogService) : INotificationHandler<RoleCreatedDomainEvent>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IMailService _emailService = emailService;
        private readonly INotificationService _notificationService = notificationService;
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

            await _notificationService.SendNotificationToUserGroupAsync(
                details: $"Role '{role.Name}' has been created by {role.CreatedBy}.",
                groupName: Role.Admin.Name);

            //Mail mail = new(
            //    subject: "Role Created",
            //    textBody: $"Role '{role.Name}' has been created.",
            //    htmlBody: $"<p>Role '{role.Name}' has been created.</p>",
            //    toList:
            //    [
            //        new(encoding: Encoding.UTF8, name: role.CreatedBy, address: role.CreatedBy)
            //    ]);

            //await _emailService.SendEmailAsync(mail);
        }
    }
}
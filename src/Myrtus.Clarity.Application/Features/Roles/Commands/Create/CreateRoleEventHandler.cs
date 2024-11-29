using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.Clarity.Application.Repositories;
using Myrtus.Clarity.Domain.Roles;
using Myrtus.Clarity.Domain.Roles.Events;
using Myrtus.Clarity.Application.Services.Mailing;
using Myrtus.Clarity.Core.Application.Abstractions.Mailing;
using Myrtus.Clarity.Core.Domain.Abstractions.Mailing;
using MimeKit;
using System.Text;

namespace Myrtus.Clarity.Application.Features.Roles.Commands.Create
{
    internal class CreateRoleEventHandler(
        IRoleRepository roleRepository,
        IMailService emailService,
        IAuditLogService auditLogService) : INotificationHandler<RoleCreatedDomainEvent>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IMailService _emailService = emailService;
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

            //    public interface IMailService
            //{
            //    void SendMail(Mail mail);
            //    Task SendEmailAsync(Mail mail);
            //}

            //            using MimeKit;

            //namespace Myrtus.Clarity.Core.Domain.Abstractions.Mailing
            //    {
            //        public class Mail
            //        {
            //            public string Subject { get; set; }
            //            public string TextBody { get; set; }
            //            public string HtmlBody { get; set; }
            //            public AttachmentCollection? Attachments { get; set; }
            //            public List<MailboxAddress> ToList { get; set; }
            //            public List<MailboxAddress>? CcList { get; set; }
            //            public List<MailboxAddress>? BccList { get; set; }
            //            public string? UnsubscribeLink { get; set; }

            //            public Mail()
            //            {
            //                Subject = string.Empty;
            //                TextBody = string.Empty;
            //                HtmlBody = string.Empty;
            //                ToList = [];
            //            }

            //            public Mail(string subject, string textBody, string htmlBody, List<MailboxAddress> toList)
            //            {
            //                Subject = subject;
            //                TextBody = textBody;
            //                HtmlBody = htmlBody;
            //                ToList = toList;
            //            }
            //        }
            //    }

            //public Mail(string subject, string textBody, string htmlBody, List<MailboxAddress> toList)

            Mail mail = new(
                subject: "Role Created",
                textBody: $"Role '{role.Name}' has been created.",
                htmlBody: $"<p>Role '{role.Name}' has been created.</p>",
                toList:
                [
                    new(encoding: Encoding.UTF8, name: role.CreatedBy, address: role.CreatedBy)
                ]);
            await _emailService.SendEmailAsync(mail);
        }
    }
}
using MediatR;
using Myrtus.Clarity.Core.Application.Abstractions.Auditing;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Abstractions.Mailing;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Blogs.Events;

namespace Myrtus.CMS.Application.Features.Blogs.Commands.Create;

internal sealed class CreateBlogEventHandler : INotificationHandler<BlogCreatedEvent>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IEmailService _emailService;
    private readonly IAuditLogService _auditLogService;

    public CreateBlogEventHandler(
        IBlogRepository blogRepository,
        IEmailService emailService,
        IAuditLogService auditLogService)
    {
        _blogRepository = blogRepository;
        _emailService = emailService;
        _auditLogService = auditLogService;
    }

    public async Task Handle(BlogCreatedEvent notification, CancellationToken cancellationToken)
    {
        Blog? blog = await _blogRepository.GetBlogByIdAsync(notification.BlogId, include: blog => blog.Owner, cancellationToken: cancellationToken);

        if (blog == null)
        {
            return;
        }

        if (blog.Owner == null)
        {
            return;
        }

        //auditLog
        //public Guid Id { get; set; } = Guid.NewGuid();
        //public string User { get; set; }
        //public string Action { get; set; }
        //public string Entity { get; set; }
        //public string EntityId { get; set; }
        //public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        //public string Details { get; set; }

        AuditLog log = new()
        {
            User = blog.Owner.Email,
            Action = "BlogCreated",
            Entity = "Blog",
            EntityId = blog.Id.ToString(),
            Details = $"Blog '{blog.Title.Value}' has been created successfully."
        };
        await _auditLogService.LogAsync(log);

        await _emailService.SendAsync(
           blog.Owner.Email,
           "Blog Created Successfully!",
           $"Your blog titled '{blog.Title.Value}' has been successfully created.");

    }
}

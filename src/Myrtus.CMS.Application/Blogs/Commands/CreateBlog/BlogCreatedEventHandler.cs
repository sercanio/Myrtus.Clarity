using MediatR;
using Myrtus.CMS.Domain.Blogs.Events;
using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Application.Repositories;
using Microsoft.Extensions.Logging;
using Myrtus.CMS.Application.Abstractions.Mailing;

namespace Myrtus.CMS.Application.Blogs.EventHandlers;

public sealed class BlogCreatedEventHandler : INotificationHandler<BlogCreatedEvent>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<BlogCreatedEvent> _logger;

    public BlogCreatedEventHandler(
        IBlogRepository blogRepository,
        IEmailService emailService,
        ILogger<BlogCreatedEvent> logger)
    {
        _blogRepository = blogRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Handle(BlogCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handling BlogCreatedEvent for BlogId: {notification.BlogId}");

        Blog? blog = await _blogRepository.GetBlogByIdAsync(notification.BlogId, include: blog => blog.Owner, cancellationToken: cancellationToken);

        if (blog == null)
        {
            _logger.LogError($"Blog with ID {notification.BlogId} not found.");
            return;
        }

        if (blog.Owner == null)
        {
            _logger.LogError($"Blog owner not found for BlogId {blog.Id}");
            return;
        }

        // Log information about the blog and its owner
        _logger.LogInformation($"A new blog '{blog.Title.Value}' created by {blog.Owner.Email.Value}");

        // Optionally send email
        // await _emailService.SendAsync(
        //    blog.Owner.Email,
        //    "Blog Created Successfully!",
        //    $"Your blog titled '{blog.Title.Value}' has been successfully created.");
    }
}

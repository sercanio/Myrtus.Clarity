using Myrtus.CMS.Application.Abstractions.Email;

namespace Myrtus.CMS.Infrastructure.Email;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(Domain.Users.Email recipient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}

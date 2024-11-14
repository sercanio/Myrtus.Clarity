using Myrtus.CMS.Application.Abstractions.Mailing;

namespace Myrtus.CMS.Infrastructure.Mailing;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(string recipient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}

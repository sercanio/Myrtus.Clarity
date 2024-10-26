using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Application.Abstractions.Mailing;

namespace Myrtus.CMS.Infrastructure.Mailing;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(Email recipient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}

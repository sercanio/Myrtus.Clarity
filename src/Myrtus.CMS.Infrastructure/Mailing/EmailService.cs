using Myrtus.Clarity.Application.Services.Mailing;

namespace Myrtus.Clarity.Infrastructure.Mailing
{
    internal sealed class EmailService : IEmailService
    {
        public Task SendAsync(string recipient, string subject, string body)
        {
            return Task.CompletedTask;
        }
    }
}

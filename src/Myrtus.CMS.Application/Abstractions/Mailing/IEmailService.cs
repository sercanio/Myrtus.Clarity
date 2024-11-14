namespace Myrtus.CMS.Application.Abstractions.Mailing;

public interface IEmailService
{
    Task SendAsync(string recipient, string subject, string body);
}

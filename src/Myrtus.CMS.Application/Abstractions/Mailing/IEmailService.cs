using Myrtus.CMS.Domain.Users;
namespace Myrtus.CMS.Application.Abstractions.Mailing;

public interface IEmailService
{
    Task SendAsync(Email recipient, string subject, string body);
}

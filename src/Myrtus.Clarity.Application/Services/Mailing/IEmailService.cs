namespace Myrtus.Clarity.Application.Services.Mailing
{
    public interface IEmailService
    {
        Task SendAsync(string recipient, string subject, string body);
    }
}

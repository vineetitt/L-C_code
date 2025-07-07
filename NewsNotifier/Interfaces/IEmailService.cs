using NewsNotifier.Models.Entities;

namespace NewsNotifier.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}

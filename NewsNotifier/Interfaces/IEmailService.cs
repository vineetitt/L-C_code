using NewsNotifier.Models.Entities;

namespace NewsNotifier.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}

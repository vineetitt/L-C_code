using NewsNotifier.Models.Entities;

namespace NewsNotifier.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(User user, NewsArticle article);
    }
}

using NewsNotifier.Models.Entities;

namespace NewsNotifier.Interfaces
{
    public interface INotificationService
    {
        void SendNotification(User user, NewsArticle article);
    }
}

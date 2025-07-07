using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Repositories.Interfaces
{
    public interface INotificationConfigRepository
    {
        Task<List<NotificationConfig>> GetAllConfigsAsync();
        Task<List<NotificationConfig>> GetConfigsByUserIdAsync(int userId);
        Task AddOrUpdateConfigAsync(NotificationConfig config);
        Task SaveOrUpdateConfigAsync(NotificationConfig config);
    }
}

using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Interfaces
{
    public interface INotificationConfigService
    {
        Task SaveOrUpdateConfigAsync(NotificationConfig config);

    }
}

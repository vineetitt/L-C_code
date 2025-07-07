using NewsAggregator.Server.Dtos.ServerDtos;
using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<ServerSummaryDto>> GetServerSummariesAsync();
        Task<IEnumerable<ExternalServer>> GetAllServersAsync();

        Task<ExternalServer?> GetServerByIdAsync(int serverId);
        Task UpdateServerAsync(ExternalServer server);

        Task<bool> AddCategoryAsync(Category category);
        Task<bool> CategoryExistsAsync(string categoryName);
        Task<List<Category>> GetAllCategoriesAsync();

        Task<NewsArticle?> GetNewsByIdAsync(int newsId);
        Task UpdateNewsAsync(NewsArticle news);
        Task DeleteNewsAsync(NewsArticle news);
    }
}

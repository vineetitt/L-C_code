using NewsAggregator.Server.Dtos.ServerDtos;
using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<ServerSummaryDto>> GetServerSummariesAsync();
        Task<IEnumerable<ExternalServer>> GetAllServersAsync();

        Task<ExternalServer?> GetServerByIdAsync(int id);
        Task UpdateServerAsync(ExternalServer server);

        Task<bool> AddCategoryAsync(Category category);
        Task<bool> CategoryExistsAsync(string categoryName);

        Task<List<Category>> GetAllCategories();

        Task<NewsArticle?> GetNewsByIdAsync(int id);
        Task UpdateNewsAsync(NewsArticle news);
        Task DeleteNewsAsync(NewsArticle news);

    }
}

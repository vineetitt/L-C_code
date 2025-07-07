using NewsAggregator.Server.Dtos.ServerDtos;
using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<ExternalServer>> GetExternalServersAsync();
        Task<IEnumerable<ServerSummaryDto>> GetServerSummariesAsync();
        Task<ExternalServer?> GetExternalServerByIdAsync(int id);
        Task<bool> UpdateServerAsync(int id, string newApiKey);
        Task<bool> AddCategoryAsync(string name);
        Task<List<Category>> GetAllCategories();
        Task<bool> UpdateNewsAsync(int id, NewsArticle updatedNews);
        Task<bool> DeleteNewsAsync(int id);

    }
}

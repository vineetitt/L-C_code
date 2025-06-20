using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<ExternalServer>> GetAllServersAsync();
        Task<ExternalServer?> GetServerByIdAsync(int id);
        Task UpdateServerAsync(ExternalServer server);

        Task<bool> AddCategoryAsync(Category category);
        Task<bool> CategoryExistsAsync(string categoryName);
    }
}

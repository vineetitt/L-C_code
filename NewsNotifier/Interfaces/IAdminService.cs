using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<ExternalServer>> GetExternalServersAsync();
        Task<ExternalServer?> GetExternalServerByIdAsync(int id);
        Task<bool> UpdateServerAsync(int id, string newApiKey);
        Task<bool> AddCategoryAsync(string name);
    }
}

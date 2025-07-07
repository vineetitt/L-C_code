using NewsAggregator.Server.Dtos.ServerDtos;
using NewsAggregator.Server.Interfaces;
using NewsAggregator.Server.Repositories.Interfaces;
using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public Task<IEnumerable<ServerSummaryDto>> GetServerSummariesAsync()
        {
            return _adminRepository.GetServerSummariesAsync();
        }

        public Task<IEnumerable<ExternalServer>> GetExternalServersAsync()
        {
            return _adminRepository.GetAllServersAsync();
        }

        public async Task<bool> UpdateServerAsync(int serverId, string newApiKey)
        {
            ValidateServerId(serverId);
            ValidateApiKey(newApiKey);

            var existingServer = await _adminRepository.GetServerByIdAsync(serverId);
            if (existingServer == null)
                return false;

            existingServer.APIKey = newApiKey.Trim();
            existingServer.LastAccessed = DateTime.UtcNow;

            await _adminRepository.UpdateServerAsync(existingServer);
            return true;
        }

        public async Task<bool> AddCategoryAsync(string categoryName)
        {
            ValidateCategoryName(categoryName);

            var categoryExists = await _adminRepository.CategoryExistsAsync(categoryName);
            if (categoryExists)
                return false;

            var newCategory = new Category { Name = categoryName.Trim() };
            await _adminRepository.AddCategoryAsync(newCategory);
            return true;
        }

        public Task<List<Category>> GetAllCategories()
        {
            return _adminRepository.GetAllCategoriesAsync();
        }

        public async Task<bool> UpdateNewsAsync(int newsId, NewsArticle updatedNews)
        {
            var existingNews = await _adminRepository.GetNewsByIdAsync(newsId);
            if (existingNews == null)
                return false;

            MapUpdatedNews(existingNews, updatedNews);

            await _adminRepository.UpdateNewsAsync(existingNews);
            return true;
        }

        public async Task<bool> DeleteNewsAsync(int newsId)
        {
            var newsToDelete = await _adminRepository.GetNewsByIdAsync(newsId);
            if (newsToDelete == null)
                return false;

            await _adminRepository.DeleteNewsAsync(newsToDelete);
            return true;
        }

        private void ValidateServerId(int serverId)
        {
            if (serverId <= 0)
                throw new ArgumentException("Server ID must be a positive integer.", nameof(serverId));
        }

        private void ValidateApiKey(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key must not be empty.", nameof(apiKey));
        }

        private void ValidateCategoryName(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name must not be empty.", nameof(categoryName));
        }

        private void MapUpdatedNews(NewsArticle existingNews, NewsArticle updatedNews)
        {
            existingNews.Title = updatedNews.Title;
            existingNews.Description = updatedNews.Description;
            existingNews.URL = updatedNews.URL;
            existingNews.CategoryID = updatedNews.CategoryID;
            existingNews.PublishedDate = updatedNews.PublishedDate;
        }
    }
}


using Microsoft.EntityFrameworkCore;
using NewsAggregator.Server.Dtos.ServerDtos;
using NewsAggregator.Server.Interfaces;
using NewsAggregator.Server.Repositories.Interfaces;
using NewsNotifier.Data;
using NewsNotifier.Models.Entities;
using System;

namespace NewsAggregator.Server.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        //public async Task<IEnumerable<ServerSummaryDto>> GetServerSummariesAsync()
        //{
        //    var servers = await _adminRepository.GetAllServersAsync();

        //    return servers.Select(s => new ServerSummaryDto
        //    {
        //        ServerId = s.ServerID,
        //        Name = s.Name,
        //        Status = s.Status
        //    });
        //}
        public Task<IEnumerable<ServerSummaryDto>> GetServerSummariesAsync()
        {
            return _adminRepository.GetServerSummariesAsync();
        }

        public Task<IEnumerable<ExternalServer>> GetExternalServersAsync()
        {
            return _adminRepository.GetAllServersAsync();
        }

        public Task<ExternalServer?> GetExternalServerByIdAsync(int ServerId)
        {
            if (ServerId<= 0)
                throw new ArgumentException("Server ID must be a positive integer.");
            return _adminRepository.GetServerByIdAsync(ServerId);
        }

        public async Task<bool> UpdateServerAsync(int ServerId, string newApiKey)
        {
            if (ServerId <= 0)
                throw new ArgumentException("Server ID must be a positive integer.", nameof(ServerId));

            if (string.IsNullOrWhiteSpace(newApiKey))
                throw new ArgumentException("API key must not be empty.", nameof(newApiKey));

            var server = await _adminRepository.GetServerByIdAsync(ServerId);
            if (server == null)
                return false;

            server.APIKey = newApiKey.Trim();
            server.LastAccessed = DateTime.UtcNow;

            await _adminRepository.UpdateServerAsync(server);
            return true;
        }

        public async Task<bool> AddCategoryAsync(string CategoryName)
        {
            if (string.IsNullOrWhiteSpace(CategoryName))
                throw new ArgumentException("Category name must not be empty.", nameof(CategoryName));

            var exists = await _adminRepository.CategoryExistsAsync(CategoryName);
            if (exists)
                return false;


            var category = new Category { Name = CategoryName.Trim() };
            await _adminRepository.AddCategoryAsync(category);
            return true;
        }

        public Task<List<Category>> GetAllCategories()
        {
            return  _adminRepository.GetAllCategories();
        }
    }
}

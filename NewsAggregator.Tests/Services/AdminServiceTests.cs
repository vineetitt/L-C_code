using Xunit;
using Moq;
using FluentAssertions;
using NewsAggregator.Server.Services;
using NewsAggregator.Server.Repositories.Interfaces;
using NewsAggregator.Server.Dtos.ServerDtos;
using NewsNotifier.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Tests.Services
{
    public class AdminServiceTests
    {
        private readonly AdminService _adminService;
        private readonly Mock<IAdminRepository> _adminRepoMock = new();

        public AdminServiceTests()
        {
            _adminService = new AdminService(_adminRepoMock.Object);
        }

        [Fact]
        public async Task GetExternalServers_ShouldReturnListOfServers()
        {
            // Arrange
            var servers = new List<ExternalServer> { new ExternalServer { ServerID = 1, APIKey = "Key123" } };
            _adminRepoMock.Setup(x => x.GetAllServersAsync()).ReturnsAsync(servers);

            // Act
            var result = await _adminService.GetExternalServersAsync();

            // Assert
            result.Should().BeEquivalentTo(servers);
        }

        [Fact]
        public async Task AddCategory_ShouldReturnFalse_WhenCategoryAlreadyExists()
        {
            // Arrange
            var categoryName = "Sports";
            _adminRepoMock.Setup(x => x.CategoryExistsAsync(categoryName)).ReturnsAsync(true);

            // Act
            var result = await _adminService.AddCategoryAsync(categoryName);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task AddCategory_ShouldReturnTrue_WhenCategoryIsNew()
        {
            // Arrange
            var categoryName = "Technology";
            _adminRepoMock.Setup(x => x.CategoryExistsAsync(categoryName)).ReturnsAsync(false);
            _adminRepoMock.Setup(x => x.AddCategoryAsync(It.IsAny<Category>())).ReturnsAsync(true);

            // Act
            var result = await _adminService.AddCategoryAsync(categoryName);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateServer_ShouldReturnFalse_WhenServerNotFound()
        {
            // Arrange
            int serverId = 1;
            _adminRepoMock.Setup(x => x.GetServerByIdAsync(serverId)).ReturnsAsync((ExternalServer)null);

            // Act
            var result = await _adminService.UpdateServerAsync(serverId, "new-api-key");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateServer_ShouldReturnTrue_WhenServerExists()
        {
            // Arrange
            int serverId = 1;
            var server = new ExternalServer { ServerID = serverId, APIKey = "old-key" };

            _adminRepoMock.Setup(x => x.GetServerByIdAsync(serverId)).ReturnsAsync(server);
            _adminRepoMock.Setup(x => x.UpdateServerAsync(server)).Returns(Task.CompletedTask);

            // Act
            var result = await _adminService.UpdateServerAsync(serverId, "1234");

            // Assert
            result.Should().BeTrue();
            server.APIKey.Should().Be("1234");
        }
    }
}

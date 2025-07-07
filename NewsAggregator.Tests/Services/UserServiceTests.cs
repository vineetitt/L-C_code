using Xunit;
using Moq;
using FluentAssertions;
using NewsNotifier.Services;
using NewsAggregator.Server.Repositories.Interfaces;
using NewsAggregator.Server.Dtos;
using NewsNotifier.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsAggregator.Tests.Services
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _userRepoMock = new();

        public UserServiceTests()
        {
            _userService = new UserService(_userRepoMock.Object);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User { UserID = userId, Email = "test@example.com" };
            _userRepoMock.Setup(x => x.GetUserById(userId)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnListOfUsers()
        {
            // Arrange
            var users = new List<User> { new User { UserID = 1 }, new User { UserID = 2 } };
            _userRepoMock.Setup(x => x.GetAllUsers()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task SaveArticle_ShouldReturnTrue_WhenArticleIsSavedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var articleId = 101;
            _userRepoMock.Setup(x => x.SaveArticle(userId, articleId)).ReturnsAsync(true);

            // Act
            var result = await _userService.SaveArticleAsync(userId, articleId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task SaveArticle_ShouldReturnFalse_WhenArticleAlreadySaved()
        {
            // Arrange
            var userId = 1;
            var articleId = 101;
            _userRepoMock.Setup(x => x.SaveArticle(userId, articleId)).ReturnsAsync(false);

            // Act
            var result = await _userService.SaveArticleAsync(userId, articleId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetSavedArticles_ShouldReturnSavedArticlesList()
        {
            // Arrange
            var userId = 1;
            var savedArticles = new List<SavedArticleDto> {
                new SavedArticleDto { ArticleID = 101, Title = "Test" }
            };

            _userRepoMock.Setup(x => x.GetSavedArticles(userId)).ReturnsAsync(savedArticles);

            // Act
            var result = await _userService.GetSavedArticlesAsync(userId);

            // Assert
            result.Should().BeEquivalentTo(savedArticles);
        }

        [Fact]
        public async Task UnsaveArticle_ShouldReturnTrue_WhenArticleIsUnSavedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var articleId = 101;

            _userRepoMock.Setup(x => x.UnsaveArticle(userId, articleId)).ReturnsAsync(true);

            // Act
            var result = await _userService.UnsaveArticleAsync(userId, articleId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UnsaveArticle_ShouldReturnFalse_WhenArticleNotFound()
        {
            // Arrange
            var userId = 1;
            var articleId = 101;

            _userRepoMock.Setup(x => x.UnsaveArticle(userId, articleId)).ReturnsAsync(false);

            // Act
            var result = await _userService.UnsaveArticleAsync(userId, articleId);

            // Assert
            result.Should().BeFalse();
        }
    }
}

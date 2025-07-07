using Xunit;
using Moq;
using FluentAssertions;
using NewsNotifier.Services;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;
using NewsAggregator.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Server.Models.Entities;
using NewsNotifier.Repositories.Interfaces;

namespace Tests.Services
{
    public class NewsArticleServiceTests
    {
        private readonly NewsArticleService _newsService;
        private readonly Mock<INewsArticleRepository> _newsRepoMock = new();
        private readonly Mock<IEmailService> _emailServiceMock = new();

        public NewsArticleServiceTests()
        {
            _newsService = new NewsArticleService(_newsRepoMock.Object, _emailServiceMock.Object);
        }

        [Fact]
        public async Task GetNewsById_ShouldReturnArticle_WhenArticleExists()
        {
            var article = new NewsArticle { ArticleID = 1, Title = "Test" };
            _newsRepoMock.Setup(x => x.GetByIdAsync(article.ArticleID)).ReturnsAsync(article);

            var result = await _newsService.GetNewsByIdAsync(article.ArticleID);

            result.Should().BeEquivalentTo(article);
        }

        [Fact]
        public async Task GetAllNews_ShouldReturnListOfArticles()
        {
            var articles = new List<NewsArticle> { new NewsArticle { ArticleID = 1 }, new NewsArticle { ArticleID = 2 } };
            _newsRepoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(articles);

            var result = await _newsService.GetAllNewsAsync();

            result.Should().BeEquivalentTo(articles);
        }

        [Fact]
        public async Task CreateNews_ShouldCallAddMethod()
        {
            var article = new NewsArticle { ArticleID = 1, Title = "Test" };

            await _newsService.CreateNewsAsync(article);

            _newsRepoMock.Verify(x => x.AddAsync(article), Times.Once);
        }

        [Fact]
        public async Task DeleteNews_ShouldCallDeleteMethod()
        {
            int articleId = 1;

            await _newsService.DeleteNewsAsync(articleId);

            _newsRepoMock.Verify(x => x.DeleteAsync(articleId), Times.Once);
        }

        [Fact]
        public async Task HideOrUnhideArticle_ShouldUpdateVisibility()
        {
            var article = new NewsArticle { ArticleID = 1, IsHidden = false };
            _newsRepoMock.Setup(x => x.GetByIdAsync(article.ArticleID)).ReturnsAsync(article);

            await _newsService.HideOrUnhideArticleAsync(article.ArticleID, true);

            article.IsHidden.Should().BeTrue();
            _newsRepoMock.Verify(x => x.UpdateAsync(article), Times.Once);
        }

        [Fact]
        public async Task HideOrUnhideCategory_ShouldUpdateCategoryVisibility()
        {
            var category = new Category { CategoryID = 1, IsHidden = false };
            _newsRepoMock.Setup(x => x.GetCategoryByIdAsync(category.CategoryID)).ReturnsAsync(category);

            await _newsService.HideOrUnhideCategoryAsync(category.CategoryID, true);

            category.IsHidden.Should().BeTrue();
            _newsRepoMock.Verify(x => x.UpdateCategoryAsync(category), Times.Once);
        }

        [Fact]
        public async Task AddBlockedKeyword_ShouldCallAddMethod()
        {
            await _newsService.AddBlockedKeywordAsync("test");

            _newsRepoMock.Verify(x => x.AddBlockedKeywordAsync(It.Is<BlockedKeyword>(b => b.Keyword == "test")), Times.Once);
        }

        [Fact]
        public async Task DeleteBlockedKeyword_ShouldCallDeleteMethod()
        {
            await _newsService.DeleteBlockedKeywordAsync("test");

            _newsRepoMock.Verify(x => x.DeleteBlockedKeywordAsync("test"), Times.Once);
        }

        [Fact]
        public async Task ReportArticle_ShouldAutoHide_WhenReportCountReachesThree()
        {
            var articleId = 1;
            var userId = 1;
            var article = new NewsArticle { ArticleID = articleId, Title = "Test Article", IsHidden = false };

            _newsRepoMock.Setup(x => x.GetByIdAsync(articleId)).ReturnsAsync(article);
            _newsRepoMock.Setup(x => x.GetReportCountAsync(articleId)).ReturnsAsync(3);

            await _newsService.ReportArticleAsync(articleId, userId);

            _newsRepoMock.Verify(x => x.UpdateAsync(It.Is<NewsArticle>(a => a.IsHidden)), Times.Once);
            _emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task ReportArticle_ShouldNotHide_WhenReportCountLessThanThree()
        {
            var articleId = 1;
            var userId = 1;
            var article = new NewsArticle { ArticleID = articleId, Title = "Test Article", IsHidden = false };

            _newsRepoMock.Setup(x => x.GetByIdAsync(articleId)).ReturnsAsync(article);
            _newsRepoMock.Setup(x => x.GetReportCountAsync(articleId)).ReturnsAsync(2);

            await _newsService.ReportArticleAsync(articleId, userId);

            _newsRepoMock.Verify(x => x.UpdateAsync(It.IsAny<NewsArticle>()), Times.Never);
            _emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetReportedArticles_ShouldReturnReportedArticles()
        {
            var reportedArticles = new List<(NewsArticle, int)>
            {
                (new NewsArticle { ArticleID = 1, Title = "Test" }, 2)
            };

            _newsRepoMock.Setup(x => x.GetReportedArticlesAsync()).ReturnsAsync(reportedArticles);

            var result = await _newsService.GetReportedArticlesAsync();

            result.Should().BeEquivalentTo(reportedArticles);
        }
    }
}

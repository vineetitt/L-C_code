using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Server.Models.Entities;
using NewsAggregator.Server.Services;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;
using NewsNotifier.Repositories.Interfaces;
using System;

namespace NewsNotifier.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _repo;
        private readonly IEmailService _emailService;

        public NewsArticleService(INewsArticleRepository repo, IEmailService emailService)
        {
            _repo = repo;
            _emailService = emailService;
        }

        public Task<NewsArticle?> GetNewsByIdAsync(int id) => _repo.GetByIdAsync(id);

        public Task<IEnumerable<NewsArticle>> GetAllNewsAsync() => _repo.GetAllAsync();

        public Task CreateNewsAsync(NewsArticle article) => _repo.AddAsync(article);

        public Task UpdateNewsAsync(NewsArticle article) => _repo.UpdateAsync(article);

        public Task DeleteNewsAsync(int id) => _repo.DeleteAsync(id);

        public async Task ReportArticleAsync(int articleId, int userId)
        {
            var report = new ReportedArticle
            {
                ArticleID = articleId,
                UserID = userId,
                ReportedAt = DateTime.UtcNow
            };

            await _repo.ReportArticleAsync(report);

            var reportCount = await _repo.GetReportCountAsync(articleId);
            var article = await _repo.GetByIdAsync(articleId);

            if (article != null)
            {
                string adminEmail = "yvineet.2411@gmail.com";
                string subject = $"Article Reported: {article.Title}";
                string body = $"The article with ID {article.ArticleID} and title '{article.Title}' has been reported by user {userId}.<br/><br/>Current report count: {reportCount}";

                await _emailService.SendEmailAsync(adminEmail, subject, body);

                if (reportCount >= 3 && !article.IsHidden)
                {
                    article.IsHidden = true;
                    await _repo.UpdateAsync(article);

                    string autoHideSubject = $"Article Auto-Hidden: {article.Title}";
                    string autoHideBody = $"The article with ID {article.ArticleID} and title '{article.Title}' has been automatically hidden after reaching {reportCount} reports.";

                    await _emailService.SendEmailAsync(adminEmail, autoHideSubject, autoHideBody);
                }
            }
        }

        public async Task<IEnumerable<(NewsArticle Article, int ReportCount)>> GetReportedArticlesAsync()
        {
            return await _repo.GetReportedArticlesAsync();
        }

        public async Task HideOrUnhideArticleAsync(int articleId, bool hide)
        {
            var article = await _repo.GetByIdAsync(articleId);
            if (article != null)
            {
                article.IsHidden = hide;
                await _repo.UpdateAsync(article);
            }
        }

        public async Task HideOrUnhideCategoryAsync(int categoryId, bool hide)
        {
            var category = await _repo.GetCategoryByIdAsync(categoryId);
            if (category != null)
            {
                category.IsHidden = hide;
                await _repo.UpdateCategoryAsync(category);
            }
        }

        public async Task AddBlockedKeywordAsync(string keyword)
        {
            var blocked = new BlockedKeyword { Keyword = keyword.ToLower() };
            await _repo.AddBlockedKeywordAsync(blocked);
        }

        public async Task<IEnumerable<string>> GetBlockedKeywordsAsync()
        {
            return await _repo.GetBlockedKeywordsAsync();
        }

        public async Task DeleteBlockedKeywordAsync(string keyword)
        {
            await _repo.DeleteBlockedKeywordAsync(keyword.ToLower());
        }


    }
}

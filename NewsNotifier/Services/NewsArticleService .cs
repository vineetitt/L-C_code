using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly INewsArticleRepository _repository;
        private readonly IEmailService _emailService;

        public NewsArticleService(INewsArticleRepository repo, IEmailService emailService)
        {
            _repository = repo;
            _emailService = emailService;
        }

        public Task<NewsArticle?> GetNewsByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<IEnumerable<NewsArticle>> GetAllNewsAsync() => _repository.GetAllAsync();

        public Task CreateNewsAsync(NewsArticle article) => _repository.AddAsync(article);

        public Task UpdateNewsAsync(NewsArticle article) => _repository.UpdateAsync(article);

        public Task DeleteNewsAsync(int id) => _repository.DeleteAsync(id);

        public async Task ReportArticleAsync(int articleId, int userId)
        {
            var report = new ReportedArticle
            {
                ArticleID = articleId,
                UserID = userId,
                ReportedAt = DateTime.UtcNow
            };

            await _repository.ReportArticleAsync(report);

            var reportCount = await _repository.GetReportCountAsync(articleId);
            var article = await _repository.GetByIdAsync(articleId);

            if (article != null)
            {
                string adminEmail = "yvineet.2411@gmail.com";
                string subject = $"Article Reported: {article.Title}";
                string body = $"The article with ID {article.ArticleID} and title '{article.Title}' has been reported by user {userId}.<br/><br/>Current report count: {reportCount}";

                await _emailService.SendEmailAsync(adminEmail, subject, body);

                if (reportCount >= 3 && !article.IsHidden)
                {
                    article.IsHidden = true;
                    await _repository.UpdateAsync(article);

                    string autoHideSubject = $"Article Auto-Hidden: {article.Title}";
                    string autoHideBody = $"The article with ID {article.ArticleID} and title '{article.Title}' has been automatically hidden after reaching {reportCount} reports.";

                    await _emailService.SendEmailAsync(adminEmail, autoHideSubject, autoHideBody);
                }
            }
        }

        public async Task<IEnumerable<(NewsArticle Article, int ReportCount)>> GetReportedArticlesAsync()
        {
            return await _repository.GetReportedArticlesAsync();
        }

        public async Task HideOrUnhideArticleAsync(int articleId, bool hide)
        {
            var article = await _repository.GetByIdAsync(articleId);
            if (article != null)
            {
                article.IsHidden = hide;
                await _repository.UpdateAsync(article);
            }
        }

        public async Task HideOrUnhideCategoryAsync(int categoryId, bool hide)
        {
            var category = await _repository.GetCategoryByIdAsync(categoryId);
            if (category != null)
            {
                category.IsHidden = hide;
                await _repository.UpdateCategoryAsync(category);
            }
        }

        public async Task AddBlockedKeywordAsync(string keyword)
        {
            var blocked = new BlockedKeyword { Keyword = keyword.ToLower() };
            await _repository.AddBlockedKeywordAsync(blocked);
        }

        public async Task<IEnumerable<string>> GetBlockedKeywordsAsync()
        {
            return await _repository.GetBlockedKeywordsAsync();
        }

        public async Task DeleteBlockedKeywordAsync(string keyword)
        {
            await _repository.DeleteBlockedKeywordAsync(keyword.ToLower());
        }


        public async Task<List<NewsArticle>> GetPersonalizedNewsAsync(List<int> categoryIds, List<string> keywords)
        {
            return await _repository.GetPersonalizedNewsAsync(categoryIds, keywords);
        } 


    }
}

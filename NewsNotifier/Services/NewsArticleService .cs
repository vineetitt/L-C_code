using NewsAggregator.Server.Models.Entities;
using NewsAggregator.Server.Services;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;
using NewsNotifier.Repositories.Interfaces;

namespace NewsNotifier.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _repository;
        private readonly IEmailService _emailService;

        public NewsArticleService(INewsArticleRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public Task<NewsArticle?> GetNewsByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<IEnumerable<NewsArticle>> GetAllNewsAsync() => _repository.GetAllAsync();

        public Task CreateNewsAsync(NewsArticle article) => _repository.AddAsync(article);

        public Task UpdateNewsAsync(NewsArticle article) => _repository.UpdateAsync(article);

        public Task DeleteNewsAsync(int id) => _repository.DeleteAsync(id);

        public async Task ReportArticleAsync(int articleId, int userId)
        {
            await _repository.ReportArticleAsync(new ReportedArticle
            {
                ArticleID = articleId,
                UserID = userId,
                ReportedAt = DateTime.UtcNow
            });

            var reportCount = await _repository.GetReportCountAsync(articleId);
            var article = await _repository.GetByIdAsync(articleId);

            if (article is null)
                return;

            await NotifyAdminOfReportedArticle(article, userId, reportCount);

            if (reportCount >= 3 && !article.IsHidden)
            {
                article.IsHidden = true;
                await _repository.UpdateAsync(article);
                await NotifyAdminOfAutoHide(article, reportCount);
            }
        }

        public Task<IEnumerable<(NewsArticle Article, int ReportCount)>> GetReportedArticlesAsync()
        {
            return _repository.GetReportedArticlesAsync();
        }

        public async Task HideOrUnhideArticleAsync(int articleId, bool hide)
        {
            var article = await _repository.GetByIdAsync(articleId);
            if (article is null)
                return;

            article.IsHidden = hide;
            await _repository.UpdateAsync(article);
        }

        public async Task HideOrUnhideCategoryAsync(int categoryId, bool hide)
        {
            var category = await _repository.GetCategoryByIdAsync(categoryId);
            if (category is null)
                return;

            category.IsHidden = hide;
            await _repository.UpdateCategoryAsync(category);
        }

        public async Task AddBlockedKeywordAsync(string keyword)
        {
            await _repository.AddBlockedKeywordAsync(new BlockedKeyword { Keyword = keyword.ToLower() });
        }

        public Task<IEnumerable<string>> GetBlockedKeywordsAsync() => _repository.GetBlockedKeywordsAsync();

        public Task DeleteBlockedKeywordAsync(string keyword) => _repository.DeleteBlockedKeywordAsync(keyword.ToLower());

        public Task<List<NewsArticle>> GetPersonalizedNewsAsync(List<int> categoryIds, List<string> keywords)
        {
            return _repository.GetPersonalizedNewsAsync(categoryIds, keywords);
        }
        private async Task NotifyAdminOfReportedArticle(NewsArticle article, int userId, int reportCount)
        {
            string subject = $"Article Reported: {article.Title}";
            string body = $"The article with ID {article.ArticleID} and title '{article.Title}' has been reported by user {userId}.<br/><br/>Current report count: {reportCount}";

            await _emailService.SendEmailAsync("yvineet.2411@gmail.com", subject, body);
        }
        private async Task NotifyAdminOfAutoHide(NewsArticle article, int reportCount)
        {
            string subject = $"Article Auto-Hidden: {article.Title}";
            string body = $"The article with ID {article.ArticleID} and title '{article.Title}' has been automatically hidden after reaching {reportCount} reports.";

            await _emailService.SendEmailAsync("yvineet.2411@gmail.com", subject, body);
        }
    }
}

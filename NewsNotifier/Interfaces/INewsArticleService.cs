
using NewsNotifier.Models.Entities;

namespace NewsNotifier.Interfaces
{
    public interface INewsArticleService
    {
        Task<NewsArticle?> GetNewsByIdAsync(int id);
        Task<IEnumerable<NewsArticle>> GetAllNewsAsync();
        Task<IEnumerable<(NewsArticle Article, int ReportCount)>> GetReportedArticlesAsync();
        Task<IEnumerable<string>> GetBlockedKeywordsAsync();
        Task<List<NewsArticle>> GetPersonalizedNewsAsync(List<int> categoryIds, List<string> keywords);

        Task CreateNewsAsync(NewsArticle article);
        Task ReportArticleAsync(int articleId, int userId);
        Task AddBlockedKeywordAsync(string keyword);
        Task UpdateNewsAsync(NewsArticle article);
        Task HideOrUnhideArticleAsync(int articleId, bool hide);
        Task HideOrUnhideCategoryAsync(int categoryId, bool hide);
        Task DeleteNewsAsync(int id);
        Task DeleteBlockedKeywordAsync(string keyword);
    }
}

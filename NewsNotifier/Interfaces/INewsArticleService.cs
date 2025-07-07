using NewsNotifier.Models.Entities;

namespace NewsNotifier.Interfaces
{
    public interface INewsArticleService
    {
        Task<NewsArticle?> GetNewsByIdAsync(int id);
        Task<IEnumerable<NewsArticle>> GetAllNewsAsync();
        Task CreateNewsAsync(NewsArticle article);
        Task UpdateNewsAsync(NewsArticle article);
        Task DeleteNewsAsync(int id);
        Task ReportArticleAsync(int articleId, int userId);

        Task<IEnumerable<(NewsArticle Article, int ReportCount)>> GetReportedArticlesAsync();

        Task HideOrUnhideArticleAsync(int articleId, bool hide);

        Task HideOrUnhideCategoryAsync(int categoryId, bool hide);

        Task AddBlockedKeywordAsync(string keyword);
        Task<IEnumerable<string>> GetBlockedKeywordsAsync();
        Task DeleteBlockedKeywordAsync(string keyword);

    }
}

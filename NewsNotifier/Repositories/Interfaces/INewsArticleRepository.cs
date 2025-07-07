using NewsAggregator.Server.Models.Entities;
using NewsNotifier.Models.Entities;

namespace NewsNotifier.Repositories.Interfaces
{
    public interface INewsArticleRepository
    {
        Task<NewsArticle?> GetByIdAsync(int id);
        Task<IEnumerable<NewsArticle>> GetAllAsync();
        Task AddAsync(NewsArticle article);
        Task UpdateAsync(NewsArticle article);
        Task DeleteAsync(int id);

        Task ReportArticleAsync(ReportedArticle report);
        Task<int> GetReportCountAsync(int articleId);

        Task<IEnumerable<(NewsArticle Article, int ReportCount)>> GetReportedArticlesAsync();

        Task<Category?> GetCategoryByIdAsync(int categoryId);
        Task UpdateCategoryAsync(Category category);

        Task AddBlockedKeywordAsync(BlockedKeyword keyword);
        Task<IEnumerable<string>> GetBlockedKeywordsAsync();
        Task DeleteBlockedKeywordAsync(string keyword);




    }
}

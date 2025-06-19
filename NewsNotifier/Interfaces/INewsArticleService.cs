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
    }
}

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
    }
}

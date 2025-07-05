using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Repositories.Interfaces
{
    public interface INewsRepository
    {
        List<NewsArticle> GetNews(int? categoryId);
        NewsArticle? GetNewsById(int id);

        List<Category> GetAllCategories();
    }
}

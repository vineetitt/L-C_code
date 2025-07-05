using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Interfaces
{
    public interface INewsService
    {
        List<NewsArticle> GetNews(int? categoryId);
        NewsArticle? GetNewsById(int id);

        List<Category> GetAllCategories();
    }
}

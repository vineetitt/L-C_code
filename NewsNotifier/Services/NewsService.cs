using Microsoft.EntityFrameworkCore;
using NewsAggregator.Server.Interfaces;
using NewsAggregator.Server.Repositories.Interfaces;
using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public List<NewsArticle> GetNews(int? categoryId)
        {
            return _newsRepository.GetNews(categoryId);
        }

        public NewsArticle? GetNewsById(int id)
        {
            return _newsRepository.GetNewsById(id);
        }

        public List<Category> GetAllCategories()
        {
            return _newsRepository.GetAllCategories();
        }

    }
}

using NewsNotifier.Models.Entities;

namespace NewsNotifier.Interfaces
{
    public interface INewsApiService
    {
        //Task<List<NewsArticle>> FetchAndSaveNewsAsync(string query, DateTime from, DateTime to);
        Task FetchAndSaveNewsAsync();
    }
}

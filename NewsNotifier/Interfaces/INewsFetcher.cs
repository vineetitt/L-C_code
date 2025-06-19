using NewsNotifier.Models.Entities;

namespace NewsNotifier.Interfaces
{
    public interface INewsFetcher
    {
        List<NewsArticle> FetchNews();
    }
}

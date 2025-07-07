using NewsNotifier.Models.Entities;

namespace NewsNotifier.Interfaces
{
    public interface INewsApiService
    {
        Task FetchAndSaveNewsAsync();
    }
}

using NewsNotifierClient.Models;
using System.Net.Http.Json;
using NewsNotifier.Models.Entities;

namespace NewsNotifierClient.Services
{
    public class AdminService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public AdminService(HttpClient client, string baseUrl)
        {
            _client = client;
            _baseUrl = baseUrl.TrimEnd('/');
        }
        public async Task GetServerSummariesAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/Admin/servers/summaries");
            if (response.IsSuccessStatusCode)
            {
                var summaries = await response.Content.ReadFromJsonAsync<List<ServerSummaryDto>>();

                if (summaries != null && summaries.Count > 0)
                {
                    Console.WriteLine("\nServer Summaries:");
                    foreach (var server in summaries)
                    {
                        Console.WriteLine($"- ID: {server.ServerId}, Name: {server.Name}, Status: {server.Status}");
                    }
                }
                else
                {
                    Console.WriteLine("No servers found.");
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        public async Task GetServersAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/Admin/servers");
            if (response.IsSuccessStatusCode)
            {
                var servers = await response.Content.ReadFromJsonAsync<List<ExternalServer>>();

                if (servers != null && servers.Count > 0)
                {
                    Console.WriteLine("\nServers:");
                    foreach (var server in servers)
                    {
                        Console.WriteLine($"- ID: {server.ServerID}, Name: {server.Name}, API Key: {server.APIKey}, Status: {server.Status}");
                    }
                }
                else
                {
                    Console.WriteLine("No servers found.");
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        public async Task GetAllCategoriesAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/Admin/categories");
            if (response.IsSuccessStatusCode)
            {
                var categories = await response.Content.ReadFromJsonAsync<List<NewsNotifierClient.Models.Category>>();
                if (categories != null && categories.Count > 0)
                {
                    Console.WriteLine("\nCategories List:");
                    foreach (var category in categories)
                    {
                        Console.WriteLine($"- {category.CategoryID}: {category.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo categories found.");
                }

            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        public async Task AddCategoryAsync(string categoryName)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/Admin/categories", new { Name = categoryName });

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Category '{categoryName}' added successfully.");
            }
            else
            {
                Console.WriteLine($"Error adding category: {response.StatusCode} - {response.ReasonPhrase}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task UpdateNewsAsync(int id)
        {
            Console.WriteLine("Enter new title:");
            var title = Console.ReadLine()!;
            Console.WriteLine("Enter new description:");
            var description = Console.ReadLine()!;
            Console.WriteLine("Enter new URL:");
            var url = Console.ReadLine()!;
            Console.WriteLine("Enter Category ID:");
            int categoryId = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter Published Date (yyyy-MM-dd):");
            DateTime publishedDate = DateTime.Parse(Console.ReadLine()!);

            var updatedNews = new
            {
                ArticleID = id,
                Title = title,
                Description = description,
                URL = url,
                CategoryID = categoryId,
                PublishedDate = publishedDate
            };

            var response = await _client.PutAsJsonAsync($"{_baseUrl}/admin/news/{id}", updatedNews);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("News updated successfully.");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        public async Task DeleteNewsAsync(int id)
        {
            var response = await _client.DeleteAsync($"{_baseUrl}/admin/news/{id}");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("News deleted successfully.");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }


        public async Task ViewReportedArticlesAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/NewsArticle/reported-articles");
            if (response.IsSuccessStatusCode)
            {
                var articles = await response.Content.ReadFromJsonAsync<List<ReportedArticleDto>>();
                Console.WriteLine("\nReported Articles:");
                foreach (var article in articles)
                {
                    Console.WriteLine($"- ID: {article.ArticleID}, Title: {article.Title}, Reports: {article.ReportCount}");
                }
            }
            else
            {
                Console.WriteLine("Error fetching reported articles.");
            }
        }

        public async Task ToggleHideArticleAsync()
        {
            Console.Write("Enter Article ID: ");
            int articleId = int.Parse(Console.ReadLine()!);
            Console.Write("Hide? (y/n): ");
            bool hide = Console.ReadLine()?.Trim().ToLower() == "y";

            var response = await _client.PostAsync($"{_baseUrl}/NewsArticle/{articleId}/toggle-hide?hide={hide}", null);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(hide ? "Article hidden." : "Article unhidden.");
            }
            else
            {
                Console.WriteLine("Error updating article visibility.");
            }
        }


        public async Task ToggleHideCategoryAsync()
        {
            Console.Write("Enter Category ID: ");
            int categoryId = int.Parse(Console.ReadLine()!);
            Console.Write("Hide? (y/n): ");
            bool hide = Console.ReadLine()?.Trim().ToLower() == "y";

            var response = await _client.PostAsync($"{_baseUrl}/NewsArticle/categories/{categoryId}/toggle-hide?hide={hide}", null);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(hide ? "Category hidden." : "Category unhidden.");
            }
            else
            {
                Console.WriteLine("Error updating category visibility.");
            }
        }

        public async Task ManageBlockedKeywordsAsync()
        {
            bool managing = true;

            while (managing)
            {
                Console.Clear();
                Console.WriteLine("\nBlocked Keywords Management");
                Console.WriteLine("1. View Blocked Keywords");
                Console.WriteLine("2. Add Blocked Keyword");
                Console.WriteLine("3. Delete Blocked Keyword");
                Console.WriteLine("4. Back to Admin Menu");
                Console.Write("Choose: ");
                var input = Console.ReadLine();

                if (input == "1")
                {
                    var response = await _client.GetAsync($"{_baseUrl}/NewsArticle/blocked-keywords");
                    if (response.IsSuccessStatusCode)
                    {
                        var keywords = await response.Content.ReadFromJsonAsync<List<string>>();
                        Console.WriteLine("\nBlocked Keywords:");
                        keywords.ForEach(k => Console.WriteLine($"- {k}"));
                    }
                    else
                    {
                        Console.WriteLine("Error fetching keywords.");
                    }
                }
                else if (input == "2")
                {
                    Console.Write("Enter keyword to block: ");
                    string keyword = Console.ReadLine()!;
                    var response = await _client.PostAsync($"{_baseUrl}/NewsArticle/blocked-keywords?keyword={keyword}", null);
                    Console.WriteLine(response.IsSuccessStatusCode ? "Keyword blocked." : "Error blocking keyword.");
                }
                else if (input == "3")
                {
                    Console.Write("Enter keyword to unblock: ");
                    string keyword = Console.ReadLine()!;
                    var response = await _client.DeleteAsync($"{_baseUrl}/NewsArticle/blocked-keywords?keyword={keyword}");
                    Console.WriteLine(response.IsSuccessStatusCode ? "Keyword unblocked." : "Error unblocking keyword.");
                }
                else if (input == "4")
                {
                    managing = false;
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                }

                if (managing)
                {
                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                }
            }
        }


    }
}

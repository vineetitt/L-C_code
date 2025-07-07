using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;
using NewsNotifier.Repositories;
using NewsNotifier.Repositories.Interfaces;
using NewsNotifierClient.Models;
using ClientCategory = NewsNotifierClient.Models.Category;
using ClientSavedArticle = NewsNotifierClient.Models.SavedArticle;

namespace NewsNotifierClient.Services
{
    public class UserService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        private readonly int _userId;


        public UserService(HttpClient client, string baseUrl, string token, int userId)
        {
            _client = client;
            _baseUrl = baseUrl.TrimEnd('/');
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _userId = userId;
        }

        public async Task ShowUserMenuAsync()
        {
            bool isUserMenuActive = true;

            while (isUserMenuActive)
            {
                Console.Clear();
                Console.WriteLine("=== USER MENU ===");
                Console.WriteLine("1. View Categories");
                Console.WriteLine("2. View All News");
                Console.WriteLine("3. View News by ID");
                Console.WriteLine("4. Save Article by ID");
                Console.WriteLine("5. View Saved Articles");
                Console.WriteLine("6. Unsave Article");
                Console.WriteLine("7. Configure Notifications");
                Console.WriteLine("8. Report Article");
                Console.WriteLine("9. Logout");
                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ShowCategoriesAsync();
                        break;
                    case "2":
                        await ShowAllNewsAsync();
                        break;
                    case "3":
                        Console.Write("Enter News ID: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                            await ShowNewsByIdAsync(id);
                        else
                            Console.WriteLine("Invalid ID format.");
                        break;
                    case "4":
                        Console.Write("Enter News ID to Save: ");
                        if (int.TryParse(Console.ReadLine(), out int saveId))
                            await SaveArticleAsync(saveId);
                        else
                            Console.WriteLine("Invalid ID format.");
                        break;
                    case "5":
                        await ViewSavedArticlesAsync();
                        break;
                    case "6":
                        await UnsaveArticleAsync();
                        break;
                    case "7":
                        await ConfigureNotificationsAsync();
                        break;
                    case "8":
                        await ReportArticleAsync();
                        break;

                    case "9":
                        Console.WriteLine("Logging out...");
                        isUserMenuActive = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                if (isUserMenuActive)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private async Task ShowCategoriesAsync()
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/user/categories");
                if (response.IsSuccessStatusCode)
                {
                    var categories = await response.Content.ReadFromJsonAsync<List<ClientCategory>>();
                    if (categories != null && categories.Count > 0)
                    {
                        Console.WriteLine("\nCategories:");
                        foreach (var c in categories)
                            Console.WriteLine($"- {c.CategoryID}: {c.Name}");
                    }
                    else
                    {
                        Console.WriteLine("No categories found.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching categories: " + ex.Message);
            }
        }

        private async Task ShowAllNewsAsync()
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/user/news");
                if (response.IsSuccessStatusCode)
                {
                    var news = await response.Content.ReadFromJsonAsync<List<NewsArticles>>();
                    if (news != null && news.Count > 0)
                    {
                        Console.WriteLine("\nNews Articles:");
                        foreach (var n in news)
                            Console.WriteLine($"- {n.ArticleID}: {n.Title}");
                    }
                    else
                    {
                        Console.WriteLine("No news articles found.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching news: " + ex.Message);
            }
        }

        private async Task ShowNewsByIdAsync(int id)
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/user/news/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var article = await response.Content.ReadFromJsonAsync<NewsArticles>();
                    if (article != null)
                    {
                        Console.WriteLine($"\nTitle: {article.Title}");
                        Console.WriteLine($"Description: {article.Description}");
                        Console.WriteLine($"URL: {article.URL}");
                    }
                    else
                    {
                        Console.WriteLine("Article not found.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching article: " + ex.Message);
            }
        }

        public async Task SaveArticleAsync(int articleId)
        {
            try
            {
                var response = await _client.PostAsync($"{_baseUrl}/user/save-article/{articleId}", null);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Article saved successfully.");
                }
                else
                {
                    Console.WriteLine($"Error: {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving article: " + ex.Message);
            }
        }

        public async Task ViewSavedArticlesAsync()
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/user/saved-articles");
                if (response.IsSuccessStatusCode)
                {
                    var savedArticles = await response.Content.ReadFromJsonAsync<List<ClientSavedArticle>>();
                    if (savedArticles != null && savedArticles.Count > 0)
                    {
                        Console.WriteLine("\nSaved Articles:");
                        foreach (var a in savedArticles)
                        {
                            Console.WriteLine($"- {a.ArticleID}: {a.Title} (Saved on: {a.SavedDate})");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No saved articles found.");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching saved articles: " + ex.Message);
            }
        }

        public async Task UnsaveArticleAsync()
        {
            Console.Write("Enter Article ID to unsave: ");
            if (!int.TryParse(Console.ReadLine(), out int articleId))
            {
                Console.WriteLine("Invalid Article ID.");
                return;
            }

            try
            {
                var response = await _client.DeleteAsync($"{_baseUrl}/user/unsave-article/{articleId}");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Article unsaved successfully.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error unsaving article: " + ex.Message);
            }
        }
        private async Task ReportArticleAsync()
        {
            try
            {
                Console.Write("Enter Article ID to Report: ");
                if (int.TryParse(Console.ReadLine(), out int articleId))
                {
                    var response = await _client.PostAsync($"{_baseUrl}/NewsArticle/{articleId}/report?userId={_userId}", null);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Article reported successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Error: {await response.Content.ReadAsStringAsync()}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid ID format.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reporting article: " + ex.Message);
            }
        }


        private async Task ConfigureNotificationsAsync()
        {
            Console.Write("Enter Category ID: ");
            if (!int.TryParse(Console.ReadLine(), out int categoryId))
            {
                Console.WriteLine("Invalid Category ID.");
                return;
            }

            Console.Write("Enter Keywords (comma separated): ");
            string keywords = Console.ReadLine() ?? "";

            Console.Write("Enable Notification? (y/n): ");
            bool isEnabled = Console.ReadLine()?.Trim().ToLower() == "y";

            var notificationConfig = new
            {
                CategoryID = categoryId,
                Keywords = keywords,
                IsEnabled = isEnabled
            };

            try
            {
                var response = await _client.PostAsJsonAsync($"{_baseUrl}/NotificationConfig/configure", notificationConfig);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("SUCCESS: Notification configuration updated successfully.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error details: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error configuring notifications: " + ex.Message);
            }
        }
    }
}

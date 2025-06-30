using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace NewsNotifierClient.Services
{
    public class UserService
    {
            private readonly HttpClient _client;
            private readonly string _baseUrl;

            public UserService(HttpClient client, string baseUrl)
            {
                _client = client;
                _baseUrl = baseUrl.TrimEnd('/');
            }

            public async Task ShowUserMenuAsync()
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("=== USER MENU ===");
                    Console.WriteLine("1. View Categories");
                    Console.WriteLine("2. View All News");
                    Console.WriteLine("3. View News by ID");
                    Console.WriteLine("4. Logout");
                    Console.Write("Choose: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1": await ShowCategoriesAsync(); break;
                        case "2": await ShowAllNewsAsync(); break;
                        case "3":
                            Console.Write("Enter News ID: ");
                            if (int.TryParse(Console.ReadLine(), out int id))
                                await ShowNewsByIdAsync(id);
                            break;
                        case "4": return;
                        default: Console.WriteLine("Invalid choice"); break;
                    }

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }

            private async Task ShowCategoriesAsync()
            {
                var response = await _client.GetAsync($"{_baseUrl}/user/categories");
                if (response.IsSuccessStatusCode)
                {
                    var categories = await response.Content.ReadFromJsonAsync<List<dynamic>>();
                    foreach (var c in categories!)
                        Console.WriteLine($"- {c.categoryID}: {c.name}");
                }
            }

            private async Task ShowAllNewsAsync()
            {
                var response = await _client.GetAsync($"{_baseUrl}/user/news");
                if (response.IsSuccessStatusCode)
                {
                    var news = await response.Content.ReadFromJsonAsync<List<dynamic>>();
                    foreach (var n in news!)
                        Console.WriteLine($"- {n.articleID}: {n.title}");
                }
            }

            private async Task ShowNewsByIdAsync(int id)
            {
                var response = await _client.GetAsync($"{_baseUrl}/user/news/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var article = await response.Content.ReadFromJsonAsync<dynamic>();
                    Console.WriteLine($"Title: {article.title}");
                    Console.WriteLine($"Description: {article.description}");
                    Console.WriteLine($"URL: {article.url}");
                }
            }
        }
}

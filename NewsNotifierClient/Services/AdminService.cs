using NewsNotifierClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using NewsNotifierClient.Models;
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

        // GET: api/Admin/servers/summaries
        //public async Task GetServerSummariesAsync()
        //{
        //    var response = await _client.GetAsync($"{_baseUrl}/Admin/servers/summaries");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var summaries = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine("\nServer Summaries:");
        //        Console.WriteLine(summaries);
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        //    }
        //}

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



        // GET: api/Admin/servers
        //public async Task GetServersAsync()
        //{
        //    var response = await _client.GetAsync($"{_baseUrl}/Admin/servers");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var servers = await response.Content.ReadAsStringAsync();
        //        Console.WriteLine("\nServers:");
        //        Console.WriteLine(servers);
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        //    }
        //}


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


        // GET: api/Admin/categories
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

        // POST: api/Admin/categories
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
    }
}

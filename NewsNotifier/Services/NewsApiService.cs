using Microsoft.EntityFrameworkCore;
using NewsNotifier.Data;
using NewsNotifier.Dtos.External;
using NewsNotifier.Models.Entities;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using NewsNotifier.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace NewsNotifier.Services
{

    public class NewsApiService : INewsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NewsApiService> _logger;
        private readonly IConfiguration _configuration;

        public NewsApiService(HttpClient httpClient, ApplicationDbContext context, ILogger<NewsApiService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }


        private static readonly Dictionary<string, List<string>> CategoryKeywords = new()
        {
            { "Technology", new() { "AI", "artificial intelligence", "technology", "software", "hardware", "computer", "robot", "tech" } },
            { "Business", new() { "stocks", "market", "CEO", "company", "startup", "IPO", "revenue", "finance", "investment" } },
            { "Sports", new() { "match", "goal", "cricket", "football", "tournament", "champion", "score" } },
            { "Politics", new() { "election", "government", "policy", "minister", "president", "senate", "vote" } },
            { "Health", new() { "vaccine", "covid", "virus", "disease", "health", "hospital", "doctor" } },
            { "General", new() { } }
        };

        private string DetermineCategoryName(string text)
        {
            foreach (var kvp in CategoryKeywords)
            {
                foreach (var keyword in kvp.Value)
                {
                    if (text.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        return kvp.Key;
                }
            }

            return "General";
        }

        public async Task FetchAndSaveNewsAsync()
        {
            var apiKey = _configuration["NewsApi:Key"];
            var url = $"https://newsapi.org/v2/top-headlines?country=us&apiKey={apiKey}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "NewsNotifierApp/1.0");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogWarning($"Failed to fetch news. Status: {response.StatusCode}. Body: {content}");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var newsItems = ParseNewsJson(json);

            var existingTitles = new HashSet<string>(
                _context.NewsArticles.Select(n => n.Title).ToList()
            );

            var newArticles = newsItems
                .Where(n => !existingTitles.Contains(n.Title))
                .ToList();

            foreach (var batch in newArticles.Chunk(25))
            {
                _context.NewsArticles.AddRange(batch);
                await _context.SaveChangesAsync();
            }

            _logger.LogInformation($"Inserted {newArticles.Count} new articles.");
        }




        private List<NewsArticle> ParseNewsJson(string json)
        {
            var result = JsonSerializer.Deserialize<NewsApiResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var newsList = new List<NewsArticle>();

            var categoryDict = _context.Categories.ToDictionary(c => c.Name, c => c.CategoryID);

            foreach (var article in result?.Articles ?? Enumerable.Empty<Article>())
            {
                var combinedText = $"{article.Title} {article.Description} {article.Content}";
                var categoryName = DetermineCategoryName(combinedText);

                var categoryId = categoryDict.ContainsKey(categoryName)
                    ? categoryDict[categoryName]
                    : categoryDict["General"];

                newsList.Add(new NewsArticle
                {
                    Title = article.Title,
                    Description = article.Description ?? string.Empty,
                    Source = article.Source?.Name,
                    URL = article.Url,
                    PublishedDate = article.PublishedAt,
                    Content = article.Content,
                    CategoryID = categoryId,
                    Likes = 0,
                    Dislikes = 0
                });
            }

            return newsList;
        }


        

    }


}
using Microsoft.EntityFrameworkCore;
using NewsNotifier.Data;
using NewsNotifier.Dtos.External;
using NewsNotifier.Models.Entities;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using NewsNotifier.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using NewsAggregator.Server.Services;
using System.Text;

namespace NewsNotifier.Services
{
    public class NewsApiService : INewsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NewsApiService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public NewsApiService(HttpClient httpClient, ApplicationDbContext context, ILogger<NewsApiService> logger, IConfiguration configuration, IEmailService emailService)
        {
            _httpClient = httpClient;
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _emailService = emailService;

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

            var server = await _context.ExternalServers.FirstOrDefaultAsync(s => s.Name == "TheNewsAPI");
            if (server != null)
            {
                server.LastAccessed = DateTime.UtcNow;
                _context.ExternalServers.Update(server);
                await _context.SaveChangesAsync();
            }


            var categoryIds = newArticles.Select(n => n.CategoryID).Distinct();
            var notificationConfigs = _context.NotificationConfigs
                .Include(nc => nc.User)
                .Where(nc => categoryIds.Contains(nc.CategoryID) && nc.IsEnabled)
                .ToList();
            var categoryWiseArticles = newArticles.GroupBy(n => n.CategoryID);

            foreach (var group in categoryWiseArticles)
            {
                var categoryId = group.Key;
                var newsForCategory = group.ToList();

                var usersToNotify = notificationConfigs
                    .Where(nc => nc.CategoryID == categoryId)
                    .Select(nc => nc.User)
                    .Distinct();

                foreach (var user in usersToNotify)
                {
                    var subject = $"Latest news in {group.First().Category?.Name ?? "your category"}";
                    var body = BuildEmailBody(newsForCategory);
                    await _emailService.SendEmailAsync(user.Email, subject, body);
                }
            }
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
        private string BuildEmailBody(List<NewsArticle> articles)
        {
            var body = new StringBuilder();
            body.AppendLine("Here are the latest news articles:");

            foreach (var article in articles)
            {
                body.AppendLine($"- {article.Title} ({article.PublishedDate:yyyy-MM-dd})");
                body.AppendLine($"  {article.Description}");
                body.AppendLine($"  Read more: {article.URL}");
                body.AppendLine();
            }

            return body.ToString();
        }

    }


}
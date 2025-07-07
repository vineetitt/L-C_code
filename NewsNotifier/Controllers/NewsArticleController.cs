using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Server.Dtos;
using NewsAggregator.Server.Interfaces;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;

namespace NewsNotifier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticleController : ControllerBase
    {
        private readonly INewsArticleService _newsService;
        private readonly INotificationConfigService _notificationService;
        private readonly IUserService _userService;

        public NewsArticleController(
            INewsArticleService newsService,
            INotificationConfigService notificationService,
            IUserService userService)
        {
            _newsService = newsService;
            _notificationService = notificationService;
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("reported-articles")]
        public async Task<IActionResult> GetReportedArticlesAsync()
        {
            var reportedArticles = await _newsService.GetReportedArticlesAsync();

            var result = reportedArticles.Select(report => new
            {
                report.Article.ArticleID,
                report.Article.Title,
                report.ReportCount
            });

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateArticleAsync(NewsArticle article)
        {
            await _newsService.CreateNewsAsync(article);
            return CreatedAtAction(nameof(GetArticleByIdAsync), new { id = article.ArticleID }, article);
        }

        [Authorize(Roles = "Admin")]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticleAsync(int id, NewsArticle article)
        {
            if (id != article.ArticleID)
                return BadRequest("Article ID mismatch.");

            await _newsService.UpdateNewsAsync(article);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticleAsync(int id)
        {
            await _newsService.DeleteNewsAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]

        [HttpPost("{id}/toggle-hide")]
        public async Task<IActionResult> ToggleHideArticleAsync(int id, [FromQuery] bool hide)
        {
            await _newsService.HideOrUnhideArticleAsync(id, hide);

            var message = hide ? "Article hidden." : "Article unhidden.";
            return Ok(new { Message = message });
        }

        [Authorize(Roles = "Admin")]

        [HttpPost("categories/{id}/toggle-hide")]
        public async Task<IActionResult> ToggleHideCategoryAsync(int id, [FromQuery] bool hide)
        {
            await _newsService.HideOrUnhideCategoryAsync(id, hide);

            var message = hide ? "Category hidden." : "Category unhidden.";
            return Ok(new { Message = message });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("blocked-keywords")]
        public async Task<IActionResult> AddBlockedKeywordAsync([FromQuery] string keyword)
        {
            await _newsService.AddBlockedKeywordAsync(keyword);
            return Ok(new { Message = "Keyword blocked successfully." });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("blocked-keywords")]
        public async Task<IActionResult> GetBlockedKeywordsAsync()
        {
            var keywords = await _newsService.GetBlockedKeywordsAsync();
            return Ok(keywords);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("blocked-keywords")]
        public async Task<IActionResult> DeleteBlockedKeywordAsync([FromQuery] string keyword)
        {
            await _newsService.DeleteBlockedKeywordAsync(keyword);
            return Ok(new { Message = "Keyword unblocked successfully." });
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleByIdAsync(int id)
        {
            var article = await _newsService.GetNewsByIdAsync(id);
            if (article == null)
                return NotFound();

            return Ok(article);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllArticlesAsync()
        {
            var articles = await _newsService.GetAllNewsAsync();
            return Ok(articles);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost("{id}/report")]
        public async Task<IActionResult> ReportArticleAsync(int id, [FromQuery] int userId)
        {
            await _newsService.ReportArticleAsync(id, userId);
            return Ok(new { Message = "Article reported successfully." });
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("personalized")]
        public async Task<IActionResult> GetPersonalizedNewsAsync()
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;
                if (userIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                int userId = int.Parse(userIdClaim);

                var preferredCategories = await GetPreferredCategoryIdsAsync(userId);
                var preferredKeywords = await GetPreferredKeywordsAsync(userId);

                var personalizedNews = await _newsService.GetPersonalizedNewsAsync(preferredCategories, preferredKeywords);

                var result = personalizedNews.Select(article => new NewsArticleDto
                {
                    ArticleID = article.ArticleID,
                    Title = article.Title ?? string.Empty,
                    Description = article.Description ?? string.Empty,
                    URL = article.URL ?? string.Empty,
                    CategoryName = article.Category?.Name ?? string.Empty
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize(Roles = "User,Admin")]
        private async Task<List<int>> GetPreferredCategoryIdsAsync(int userId)
        {
            var configs = await _notificationService.GetConfigsByUserIdAsync(userId);
            var activeConfigs = configs.Where(c => c.IsEnabled).ToList();

            var preferredCategoryIds = activeConfigs.Select(c => c.CategoryID).ToList();
            var savedArticles = await _userService.GetSavedArticlesAsync(userId);

            var savedCategoryIds = savedArticles.Select(a => a.CategoryID).Distinct().ToList();

            return preferredCategoryIds.Union(savedCategoryIds).Distinct().ToList();
        }

        [Authorize(Roles = "User,Admin")]

        private async Task<List<string>> GetPreferredKeywordsAsync(int userId)
        {
            var configs = await _notificationService.GetConfigsByUserIdAsync(userId);
            var activeConfigs = configs.Where(c => c.IsEnabled).ToList();

            var preferredKeywords = activeConfigs
                .SelectMany(c => c.Keywords?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>())
                .Select(k => k.ToLower())
                .ToList();

            var savedArticles = await _userService.GetSavedArticlesAsync(userId);

            var savedKeywords = savedArticles
                .SelectMany(a => $"{a.Title ?? ""} {a.Description ?? ""}".Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(k => k.ToLower())
                .Where(k => k.Length > 3)
                .Distinct()
                .ToList();

            return preferredKeywords.Union(savedKeywords).Distinct().ToList();
        }
    }
}

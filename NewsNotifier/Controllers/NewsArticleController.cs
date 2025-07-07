using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Server.Dtos;
using NewsAggregator.Server.Interfaces;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;
using NewsNotifier.Services;

namespace NewsNotifier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticleController : ControllerBase
    {
        private readonly INewsArticleService _service;
        private readonly INotificationConfigService _notificationConfigService;
        private readonly IUserService _userService;

        public NewsArticleController(INewsArticleService service, INotificationConfigService notificationConfigService, IUserService userService)
        {
            _service = service;
            _notificationConfigService = notificationConfigService;
            _userService = userService;
        }


        [HttpGet("reported-articles")]
        public async Task<IActionResult> GetReportedArticles()
        {
            var reportedArticles = await _service.GetReportedArticlesAsync();
            var result = reportedArticles.Select(r => new
            {
                ArticleID = r.Article.ArticleID,
                Title = r.Article.Title,
                ReportCount = r.ReportCount
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var article = await _service.GetNewsByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articles = await _service.GetAllNewsAsync();
            return Ok(articles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsArticle article)
        {
            await _service.CreateNewsAsync(article);
            return CreatedAtAction(nameof(Get), new { id = article.ArticleID }, article);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NewsArticle article)
        {
            if (id != article.ArticleID)
            {
                return BadRequest();
            }

            await _service.UpdateNewsAsync(article);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteNewsAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/report")]
        public async Task<IActionResult> ReportArticle(int id, [FromQuery] int userId)
        {
            await _service.ReportArticleAsync(id, userId);
            return Ok(new { Message = "Article reported successfully." });
        }


        [HttpPost("{id}/toggle-hide")]
        public async Task<IActionResult> ToggleHideArticle(int id, [FromQuery] bool hide)
        {
            await _service.HideOrUnhideArticleAsync(id, hide);
            return Ok(new { Message = hide ? "Article hidden." : "Article unhidden." });
        }

        [HttpPost("categories/{id}/toggle-hide")]
        public async Task<IActionResult> ToggleHideCategory(int id, [FromQuery] bool hide)
        {
            await _service.HideOrUnhideCategoryAsync(id, hide);
            return Ok(new { Message = hide ? "Category hidden." : "Category unhidden." });
        }

        [HttpPost("blocked-keywords")]
        public async Task<IActionResult> AddBlockedKeyword([FromQuery] string keyword)
        {
            await _service.AddBlockedKeywordAsync(keyword);
            return Ok(new { Message = "Keyword blocked successfully." });
        }

        [HttpGet("blocked-keywords")]
        public async Task<IActionResult> GetBlockedKeywords()
        {
            var keywords = await _service.GetBlockedKeywordsAsync();
            return Ok(keywords);
        }

        [HttpDelete("blocked-keywords")]
        public async Task<IActionResult> DeleteBlockedKeyword([FromQuery] string keyword)
        {
            await _service.DeleteBlockedKeywordAsync(keyword);
            return Ok(new { Message = "Keyword unblocked successfully." });
        }

        [HttpGet("personalized")]
        public async Task<IActionResult> GetPersonalizedNews()
        {
            try
            {
                var userIdClaim = User.FindFirst("id")?.Value;

                if (userIdClaim == null)
                    return Unauthorized("User ID not found in token.");

                int userId = int.Parse(userIdClaim);

                var configs = await _notificationConfigService.GetConfigsByUserIdAsync(userId);
                var activeConfigs = configs.Where(c => c.IsEnabled).ToList();

                var preferredCategoryIds = activeConfigs.Select(c => c.CategoryID).ToList();

                var preferredKeywords = activeConfigs
                    .SelectMany(c => c.Keywords?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? new string[] { })
                    .Select(k => k.ToLower())
                    .ToList();

                var savedArticles = await _userService.GetSavedArticlesAsync(userId);

                var savedCategoryIds = savedArticles
                    .Select(a => a.CategoryID)
                    .Distinct()
                    .ToList();

                var savedKeywords = savedArticles
                    .SelectMany(a => $"{a.Title ?? ""} {a.Description ?? ""}".Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    .Select(k => k.ToLower())
                    .Where(k => k.Length > 3)
                    .Distinct()
                    .ToList();

                var finalCategoryIds = preferredCategoryIds
                    .Union(savedCategoryIds)
                    .Distinct()
                    .ToList();

                var finalKeywords = preferredKeywords
                    .Union(savedKeywords)
                    .Distinct()
                    .ToList();

                var personalizedNews = await _service.GetPersonalizedNewsAsync(finalCategoryIds, finalKeywords);
                var result = personalizedNews.Select(article => new NewsArticleDto
                {
                    ArticleID = article.ArticleID,
                    Title = article.Title ?? "",
                    Description = article.Description ?? "",
                    URL = article.URL ?? "",
                    CategoryName = article.Category?.Name ?? ""
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}

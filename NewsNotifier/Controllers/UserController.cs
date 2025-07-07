using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Server.Interfaces;

namespace NewsNotifier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IUserService _userService;

        public UserController(INewsService newsService, IUserService userService)
        {
            _newsService = newsService;
            _userService = userService;
        }

        [HttpGet("news")]
        public IActionResult GetNews([FromQuery] int? categoryId)
        {
            var newsList = _newsService.GetNews(categoryId)
                .Select(n => new
                {
                    n.ArticleID,
                    n.Title,
                    n.Description,
                    n.URL,
                    Category = n.Category?.Name,
                    n.PublishedDate
                })
                .ToList();

            return Ok(newsList);
        }

        [HttpGet("news/{id}")]
        public IActionResult GetNewsById(int id)
        {
            var article = _newsService.GetNewsById(id);

            if (article == null)
                return NotFound();

            var result = new
            {
                article.ArticleID,
                article.Title,
                article.Description,
                article.URL,
                Category = article.Category?.Name,
                article.PublishedDate
            };

            return Ok(result);
        }

        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var categories = _newsService.GetAllCategories()
                .Select(c => new
                {
                    c.CategoryID,
                    c.Name
                })
                .ToList();

            return Ok(categories);
        }

        [HttpGet("accessible-apis")]
        public IActionResult GetAccessibleApis()
        {
            var accessibleApis = new List<string>
            {
                "GET /api/user/categories",
                "GET /api/user/news",
                "GET /api/user/news/{id}"
            };

            return Ok(accessibleApis);
        }

        [HttpPost("save-article/{articleId}")]
        public async Task<IActionResult> SaveArticle(int articleId)
        {
            var userId = int.Parse(User.FindFirst("id")!.Value);

            var success = await _userService.SaveArticleAsync(userId, articleId);
            if (!success)
                return BadRequest("Article already saved or not found.");

            return Ok("Article saved successfully.");
        }

        [HttpGet("saved-articles")]
        public async Task<IActionResult> GetSavedArticles()
        {
            var userId = int.Parse(User.FindFirst("id")!.Value);

            var savedArticles = await _userService.GetSavedArticlesAsync(userId);

            return Ok(savedArticles);
        }

        [HttpDelete("unsave-article/{articleId}")]
        public async Task<IActionResult> UnsaveArticle(int articleId)
        {
            var userId = int.Parse(User.FindFirst("id")!.Value);

            var success = await _userService.UnsaveArticleAsync(userId, articleId);

            if (!success)
                return NotFound("Saved article not found.");

            return Ok("Article unsaved successfully.");
        }
    }
}

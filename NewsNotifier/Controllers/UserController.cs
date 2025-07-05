using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Server.Interfaces;
using NewsNotifier.Data;
using NewsNotifier.Models.Entities;
using NewsNotifier.Repositories.Interfaces;

namespace NewsNotifier.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly INewsService _newsService;

        public UserController(INewsService newsService)
        {
            _newsService = newsService;
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
    }
}


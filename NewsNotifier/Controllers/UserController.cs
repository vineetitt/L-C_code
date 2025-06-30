using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("news")]
        public IActionResult GetNews([FromQuery] int? categoryId)
        {
            var query = _context.NewsArticles.AsQueryable();

            //if (categoryId.HasValue)
            //{
            //    query = query.Where(n => n.CategoryID == categoryId.Value);
            //}

            var newsList = query
                .Select(n => new
                {
                    n.ArticleID,
                    n.Title,
                    n.Description,
                    n.URL,
                    //Category = n.Category.Name,
                    n.PublishedDate
                })
                .ToList();

            return Ok(newsList);
        }

        [HttpGet("news/{id}")]
        public IActionResult GetNewsById(int id)
        {
            var article = _context.NewsArticles
                .Where(n => n.ArticleID == id)
                .Select(n => new
                {
                    n.ArticleID,
                    n.Title,
                    n.Description,
                    n.URL,
                    //Category = n.Category.Name,
                    n.PublishedDate
                })
                .FirstOrDefault();

            if (article == null)
                return NotFound();

            return Ok(article);
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


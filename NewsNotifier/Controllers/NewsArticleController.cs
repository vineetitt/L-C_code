using Microsoft.AspNetCore.Mvc;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;

namespace NewsNotifier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticleController : ControllerBase
    {
        private readonly INewsArticleService _service;

        public NewsArticleController(INewsArticleService service)
        {
            _service = service;
        }


        [HttpGet("reported-articles")]
        public async Task<IActionResult> GetReportedArticles()
        {
            var reportedArticles = await _service.GetReportedArticlesAsync();
            var result = reportedArticles.Select(r => new
            {
                ArticleID = r.Article.ArticleID,  // This should match your tuple property name
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



    }
}

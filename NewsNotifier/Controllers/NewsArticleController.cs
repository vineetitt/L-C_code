using Microsoft.AspNetCore.Mvc;
using NewsNotifier.Interfaces;
using NewsNotifier.Models.Entities;

namespace NewsNotifier.Controllers
{
    public class NewsArticleController : ControllerBase
    {
        private readonly INewsArticleService _service;

        public NewsArticleController(INewsArticleService service)
        {
            _service = service;
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
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Server.Dtos.AdminDtos;
using NewsAggregator.Server.Interfaces;
using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("servers/summaries")]
        public async Task<IActionResult> GetExternalServerSummaries()
        {
            var summaries = await _adminService.GetServerSummariesAsync();
            return Ok(summaries);
        }

        [HttpGet("servers")]
        public async Task<IActionResult> GetExternalServers()
        {
            var servers = await _adminService.GetExternalServersAsync();
            return Ok(servers);
        }

        [HttpPut("servers/{id}")]
        public async Task<IActionResult> UpdateServer(int id, [FromBody] UpdateServerRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ApiKey))
                return BadRequest(new { Message = "API key is required." });

            var updated = await _adminService.UpdateServerAsync(id, request.ApiKey);
            if (!updated)
                return NotFound(new { Message = $"Server with ID {id} not found." });

            return Ok(new { Message = $"Server {id} updated successfully." });
        }

        [HttpPost("categories")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { Message = "Category name is required." });

            var added = await _adminService.AddCategoryAsync(request.Name);
            if (!added)
                return Conflict(new { Message = $"Category '{request.Name}' already exists." });

            return Ok(new { Message = $"Category '{request.Name}' added successfully." });
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _adminService.GetAllCategories();
            return Ok(categories);
        }

        [HttpPut("news/{id}")]
        public async Task<IActionResult> UpdateNews(int id, [FromBody] NewsArticle updatedNews)
        {
            var updated = await _adminService.UpdateNewsAsync(id, updatedNews);
            if (!updated)
                return NotFound(new { Message = "News not found." });

            return Ok(new { Message = "News updated successfully." });
        }

        [HttpDelete("news/{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var deleted = await _adminService.DeleteNewsAsync(id);
            if (!deleted)
                return NotFound(new { Message = "News not found." });

            return Ok(new { Message = "News deleted successfully." });
        }
    }
}

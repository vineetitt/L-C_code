using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Server.Dtos.AdminDtos;
using NewsAggregator.Server.Interfaces;
using NewsNotifier.Models.Entities;

namespace NewsAggregator.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
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
            try
            {
                var summaries = await _adminService.GetServerSummariesAsync();
                return Ok(summaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving server list: {ex.Message}");
            }
        }


        // GET: api/Admin/servers
        [HttpGet("servers")]
        public async Task<IActionResult> GetExternalServers()
        {
            try
            {
                var servers = await _adminService.GetExternalServersAsync();
                return Ok(servers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving servers: {ex.Message}");
            }
        }

        // PUT: api/Admin/servers/{id}
        [HttpPut("servers/{id}")]
        public async Task<IActionResult> UpdateServer(int id, [FromBody] UpdateServerRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ApiKey))
                return BadRequest("API key is required.");

            try
            {
                var result = await _adminService.UpdateServerAsync(id, request.ApiKey);
                if (!result)
                    return NotFound($"Server with ID {id} not found.");

                return Ok(new { Message = $"Server {id} updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the server: {ex.Message}");
            }
        }

        // POST: api/Admin/categories
        [HttpPost("categories")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Category name is required.");

            try
            {
                var added = await _adminService.AddCategoryAsync(request.Name);
                if (!added)
                    return Conflict($"Category '{request.Name}' already exists.");

                return Ok(new { Message = $"Category '{request.Name}' added successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the category: {ex.Message}");
            }
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _adminService.GetAllCategories();
              
            return Ok(categories);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NewsAggregator.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        [HttpGet("servers")]
        public IActionResult GetExternalServers()
        {
            // TODO: Fetch from DB
            return Ok(new[]
            {
                new { Id = 1, Name = "News API", Status = "Active" },
                new { Id = 2, Name = "The News API", Status = "Not Active" }
            });
        }

        [HttpPut("servers/{id}")]
        public IActionResult UpdateServer(int id, [FromBody] UpdateServerRequest request)
        {
            // TODO: Update server in DB
            return Ok(new { Message = $"Server {id} updated successfully" });
        }

        // Example: Add a new category
        [HttpPost("categories")]
        public IActionResult AddCategory([FromBody] AddCategoryRequest request)
        {
            // TODO: Add to DB
            return Ok(new { Message = $"Category {request.Name} added successfully" });
        }

        public class UpdateServerRequest
        {
            public string ApiKey { get; set; }
        }

        public class AddCategoryRequest
        {
            public string Name { get; set; }
        }

    }
}

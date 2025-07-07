using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Server.Dtos;
using NewsAggregator.Server.Interfaces;
using NewsNotifier.Models.Entities;
namespace NewsAggregator.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class NotificationConfigController : ControllerBase
    {
        private readonly INotificationConfigService _notificationService;

        public NotificationConfigController(INotificationConfigService notificationService)
        {
            _notificationService = notificationService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("configure")]
        public async Task<IActionResult> ConfigureNotification([FromBody] NotificationConfigRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst("id")?.Value;

            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            int userId = int.Parse(userIdClaim);

            var config = new NotificationConfig
            {
                UserID = userId,
                CategoryID = request.CategoryID,
                Keywords = request.Keywords,
                IsEnabled = request.IsEnabled
            };

            await _notificationService.SaveOrUpdateConfigAsync(config);

            return Ok(new { Message = "Notification configuration saved successfully." });
        }
    }
}





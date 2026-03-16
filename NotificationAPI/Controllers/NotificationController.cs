using Microsoft.AspNetCore.Mvc;
using NotificationSignalR;

namespace NotificationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("send-to-user/{userId}")]
        public async Task<IActionResult> SendToUser(string userId, [FromBody] NotificationRequest request)
        {
            try
            {
                await _notificationService.SendNotificationToUserAsync(userId, request.Message, request.Title);
                return Ok(new { message = "Notification sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("broadcast")]
        public async Task<IActionResult> Broadcast([FromBody] NotificationRequest request)
        {
            try
            {
                await _notificationService.SendNotificationToAllAsync(request.Message, request.Title);
                return Ok(new { message = "Broadcast notification sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("send-to-group/{groupName}")]
        public async Task<IActionResult> SendToGroup(string groupName, [FromBody] NotificationRequest request)
        {
            try
            {
                await _notificationService.SendNotificationToGroupAsync(groupName, request.Message);
                return Ok(new { message = "Group notification sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "NotificationAPI is healthy" });
        }
    }

    public class NotificationRequest
    {
        public string Title { get; set; } = "Notification";
        public string Message { get; set; } = string.Empty;
    }
}
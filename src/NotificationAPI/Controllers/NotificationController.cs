using Microsoft.AspNetCore.Mvc;
using NotificationSignalR;

namespace NotificationAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationPublisher _notificationPublisher;

    public NotificationController(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    [HttpPost("broadcast")]
    public async Task<IActionResult> Broadcast([FromBody] NotificationRequest request, CancellationToken cancellationToken)
    {
        var notification = request.ToNotification();
        await _notificationPublisher.PublishToAllAsync(notification, cancellationToken);
        return Ok(new { message = "Broadcast notification sent.", notification });
    }

    [HttpPost("user/{userId}")]
    public async Task<IActionResult> SendToUser(string userId, [FromBody] NotificationRequest request, CancellationToken cancellationToken)
    {
        var notification = request.ToNotification(userId: userId);
        await _notificationPublisher.PublishToUserAsync(userId, notification, cancellationToken);
        return Ok(new { message = $"Notification sent to user {userId}.", notification });
    }

    [HttpPost("group/{groupName}")]
    public async Task<IActionResult> SendToGroup(string groupName, [FromBody] NotificationRequest request, CancellationToken cancellationToken)
    {
        var notification = request.ToNotification(groupName: groupName);
        await _notificationPublisher.PublishToGroupAsync(groupName, notification, cancellationToken);
        return Ok(new { message = $"Notification sent to group {groupName}.", notification });
    }
}

public sealed record NotificationRequest(string Title, string Message)
{
    public NotificationMessage ToNotification(string? userId = null, string? groupName = null)
        => new(Title, Message, DateTimeOffset.UtcNow, userId, groupName);
}

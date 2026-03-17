using Microsoft.AspNetCore.SignalR;

namespace NotificationSignalR;

public class SignalRNotificationPublisher : INotificationPublisher
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationPublisher(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task PublishToAllAsync(NotificationMessage notification, CancellationToken cancellationToken = default)
    {
        return _hubContext.Clients.All.SendAsync("notificationReceived", notification, cancellationToken);
    }

    public Task PublishToUserAsync(string userId, NotificationMessage notification, CancellationToken cancellationToken = default)
    {
        return _hubContext.Clients.User(userId).SendAsync("notificationReceived", notification, cancellationToken);
    }

    public Task PublishToGroupAsync(string groupName, NotificationMessage notification, CancellationToken cancellationToken = default)
    {
        return _hubContext.Clients.Group(groupName).SendAsync("notificationReceived", notification, cancellationToken);
    }
}

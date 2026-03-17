namespace NotificationSignalR;

public interface INotificationPublisher
{
    Task PublishToAllAsync(NotificationMessage notification, CancellationToken cancellationToken = default);
    Task PublishToUserAsync(string userId, NotificationMessage notification, CancellationToken cancellationToken = default);
    Task PublishToGroupAsync(string groupName, NotificationMessage notification, CancellationToken cancellationToken = default);
}

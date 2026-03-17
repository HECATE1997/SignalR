namespace NotificationSignalR;

public sealed record NotificationMessage(
    string Title,
    string Message,
    DateTimeOffset Timestamp,
    string? UserId = null,
    string? GroupName = null);

using Microsoft.AspNetCore.SignalR;

namespace NotificationSignalR;

public class NotificationHub : Hub
{
    public const string HubPath = "/hubs/notifications";

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}

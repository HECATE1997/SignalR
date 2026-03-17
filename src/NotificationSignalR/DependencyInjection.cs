using Microsoft.Extensions.DependencyInjection;

namespace NotificationSignalR;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationSignalR(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddScoped<INotificationPublisher, SignalRNotificationPublisher>();
        return services;
    }
}

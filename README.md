# SignalR Notifications Microservices (.NET 8 + React 17)

This repository contains a clean microservices setup with:

- **GatewayAPI**: ASP.NET Core 8 Ocelot gateway.
- **NotificationAPI**: ASP.NET Core 8 Web API microservice.
- **NotificationSignalR**: class library containing SignalR hub + publisher abstraction.
- **react-client**: React 17 application that listens for live notifications via SignalR.

## Versions used

- .NET: **8.0** (`net8.0`, `global.json` points to SDK `8.0.100`)
- Ocelot: **23.2.2**
- Swagger for Ocelot: **8.3.1**
- React: **17.0.2**
- SignalR JS client: **8.0.7**

## Architecture

```text
React 17 Client
   |
   |  SignalR/Web API calls
   v
GatewayAPI (Ocelot, http://localhost:7000)
   |
   +--> /notification/* -> NotificationAPI (http://localhost:5071)
   +--> /hubs/notifications -> NotificationAPI SignalR hub
```

`NotificationAPI` injects `INotificationPublisher` from the `NotificationSignalR` class library through DI.
When a notification endpoint is triggered, the API publishes to the SignalR hub.

## Run order

1. Start Notification API
   ```bash
   dotnet run --project src/NotificationAPI
   ```
2. Start Gateway
   ```bash
   dotnet run --project src/GatewayAPI
   ```
3. Start React client
   ```bash
   cd src/react-client
   npm install
   npm start
   ```

## Trigger notifications through gateway

Broadcast:

```bash
curl -X POST http://localhost:7000/notification/notification/broadcast \
  -H "Content-Type: application/json" \
  -d '{"title":"System","message":"Hello all users"}'
```

User-specific:

```bash
curl -X POST http://localhost:7000/notification/notification/user/john \
  -H "Content-Type: application/json" \
  -d '{"title":"Private","message":"Hello John"}'
```

Group notification:

```bash
curl -X POST http://localhost:7000/notification/notification/group/admins \
  -H "Content-Type: application/json" \
  -d '{"title":"Admins","message":"Server restart at 22:00"}'
```

## Key files

- `src/NotificationSignalR/DependencyInjection.cs`: SignalR + publisher registration.
- `src/NotificationSignalR/NotificationHub.cs`: reusable hub.
- `src/NotificationAPI/Program.cs`: DI setup + hub mapping.
- `src/NotificationAPI/Controllers/NotificationController.cs`: event-trigger endpoints.
- `src/GatewayAPI/ocelot.json`: route + websocket proxy config.
- `src/react-client/src/App.js`: SignalR connection and UI.

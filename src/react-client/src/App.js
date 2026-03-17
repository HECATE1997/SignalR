import React, { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';

const hubUrl = 'http://localhost:7000/hubs/notifications';

function App() {
  const [notifications, setNotifications] = useState([]);
  const [connectionState, setConnectionState] = useState('Connecting...');

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl)
      .withAutomaticReconnect()
      .build();

    connection.on('notificationReceived', (message) => {
      setNotifications((current) => [message, ...current]);
    });

    connection
      .start()
      .then(() => setConnectionState('Connected'))
      .catch(() => setConnectionState('Disconnected'));

    connection.onreconnecting(() => setConnectionState('Reconnecting...'));
    connection.onreconnected(() => setConnectionState('Connected'));
    connection.onclose(() => setConnectionState('Disconnected'));

    return () => {
      connection.stop();
    };
  }, []);

  return (
    <div className="container">
      <h1>SignalR Notification Client</h1>
      <p className="status">Gateway connection: {connectionState}</p>
      <p>Trigger notifications via Notification API endpoints through Ocelot gateway.</p>

      <ul className="notification-list">
        {notifications.map((notification, index) => (
          <li key={`${notification.timestamp}-${index}`}>
            <h3>{notification.title}</h3>
            <p>{notification.message}</p>
            <small>{new Date(notification.timestamp).toLocaleString()}</small>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;

// services/socketService.ts
import * as SignalR from '@microsoft/signalr';
import { useNotificationStore } from '../store/notificationStore';

let connection: SignalR.HubConnection | null = null;

export const initSocket = (userId: string) => {
  if (connection) return connection;

  connection = new SignalR.HubConnectionBuilder()
    .withUrl(`http://192.168.0.13:6004/notification-service/hubs/notifications`) // bez tokena
    .withAutomaticReconnect()
    .build();

  // Handler za dolazne notifikacije
  connection.on('ReceiveNotification', (notification) => {
    console.log('📩 Nova notifikacija stigla:', notification);
    useNotificationStore.getState().addNotification(notification);
  });

  connection
    .start()
    .then(async () => {
      console.log('✅ SignalR connected without auth');

      // Registruj korisnika sa serverom
      await connection!.invoke('RegisterUserAsync', userId);
      console.log(`User ${userId} registered on hub`);
    })
    .catch((err) => console.error('❌ SignalR error:', err));

  return connection;
};

export const stopSocket = async () => {
  if (connection) {
    await connection.stop();
    connection = null;
  }
};

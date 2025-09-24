// stores/notificationStore.ts
import { create } from 'zustand';

interface Notification {
  id: string;
  userId: string;
  title: string;
  message: string;
  isRead: boolean;
  createdAt: string;
  type?: string;
}

interface NotificationStore {
  notifications: Notification[];
  unreadCount: number;
  isLoading: boolean;
  error: string | null;
  
  // Akcije
  fetchNotifications: (userId: string) => Promise<void>;
  createNotification: (notification: Omit<Notification, 'id' | 'createdAt' | 'isRead'>) => Promise<void>;
  markAsRead: (notificationId: string) => Promise<void>;
  markAllAsRead: (userId: string) => Promise<void>;
  clearNotifications: () => void;
  addNotification: (notification: Notification) => void;
}

const API_BASE_URL = 'http://192.168.0.13:6004/notification-service';

export const useNotificationStore = create<NotificationStore>((set) => ({
  notifications: [],
  unreadCount: 0,
  isLoading: false,
  error: null,

  // Dohvaćanje notifikacija
  fetchNotifications: async (userId: string) => {
    set({ isLoading: true, error: null });
    try {
      const response = await fetch(`${API_BASE_URL}/api/users/${userId}/notifications`);
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const notifications = await response.json();
      
      set({ 
        notifications,
        unreadCount: notifications.filter((n: Notification) => !n.isRead).length,
        isLoading: false 
      });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to fetch notifications',
        isLoading: false 
      });
    }
  },
addNotification: (notification) => set(state => ({
  notifications: [notification, ...state.notifications],
  unreadCount: state.unreadCount + 1,
})),

  // Kreiranje notifikacije
  createNotification: async (notification) => {
    set({ isLoading: true, error: null });
    try {
      const response = await fetch(`${API_BASE_URL}/api/notifications`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(notification),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const newNotification = await response.json();
      
      set((state) => ({
        notifications: [newNotification, ...state.notifications],
        unreadCount: state.unreadCount + 1,
        isLoading: false
      }));
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to create notification',
        isLoading: false 
      });
    }
  },

  // Oznaka notifikacije kao pročitane
  markAsRead: async (notificationId: string) => {
    set({ isLoading: true, error: null });
    try {
      const response = await fetch(`${API_BASE_URL}/api/notifications/${notificationId}/read`, {
        method: 'PATCH',
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      set((state) => {
        const updatedNotifications = state.notifications.map(n => 
          n.id === notificationId ? { ...n, isRead: true } : n
        );
        
        return {
          notifications: updatedNotifications,
          unreadCount: updatedNotifications.filter(n => !n.isRead).length,
          isLoading: false
        };
      });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to mark notification as read',
        isLoading: false 
      });
    }
  },

  // Oznaka svih notifikacija kao pročitane
  markAllAsRead: async (userId: string) => {
    set({ isLoading: true, error: null });
    try {
      const response = await fetch(`${API_BASE_URL}/api/notifications/mark-all-read`, {
        method: 'PATCH',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ userId }),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      set((state) => {
        const updatedNotifications = state.notifications.map(n => 
          n.userId === userId ? { ...n, isRead: true } : n
        );
        
        return {
          notifications: updatedNotifications,
          unreadCount: 0,
          isLoading: false
        };
      });
    } catch (error) {
      set({ 
        error: error instanceof Error ? error.message : 'Failed to mark all notifications as read',
        isLoading: false 
      });
    }
  },

  // Čišćenje notifikacija
  clearNotifications: () => {
    set({ notifications: [], unreadCount: 0 });
  }
}));
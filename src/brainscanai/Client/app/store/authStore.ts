import { create } from 'zustand';
import * as SecureStore from 'expo-secure-store';
import { router } from 'expo-router';
import { jwtDecode } from 'jwt-decode';

interface User {
  id: string;
  email: string;
  name: string;
  role: string;
}

interface AuthState {
  user: User | null;
  token: string | null;
  email: string | null;
  isLoading: boolean;
  error: string | null;
  login: (email: string, password: string) => Promise<void>;
  verify2FA: (code: string) => Promise<void>;
  logout: () => void;
  initializeAuth: () => Promise<void>;
  clearError: () => void;
}

export interface MyJwtPayload {
  sub: string;
  jti: string;
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string;
  UserId: string;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
  exp: number;
  iss: string;
  aud: string;
  ProfileImage:string;
}

export const useAuthStore = create<AuthState>((set, get) => ({
  user: null,
  token: null,
  email: null,
  isLoading: false,
  error: null,

  initializeAuth: async () => {
    try {
      const storedToken = await SecureStore.getItemAsync('auth_token');
      if (storedToken) {
        
        set({ token: storedToken });
      }
    } catch (error) {
      console.error('Failed to initialize auth:', error);
    } finally {
      set({ isLoading: false });
    }
  },

  login: async (email, password) => {
    set({ isLoading: true, error: null });

    try {
      const response = await fetch("http://192.168.0.13:6004/auth-service/auth/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({ email, password })
      });

      if (response.status === 200) {
        set({ email }); 
        router.push("/(screen)/Verify2FAScreen");
        return;
      }

      const errorData = await response.json();
      throw new Error(errorData.message || "Login failed");

    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : "Login failed";
      set({ error: errorMessage });
      throw err;
    } finally {
      set({ isLoading: false });
    }
  },

  verify2FA: async (code) => {
    set({ isLoading: true, error: null });

    try {
      const email = get().email;
      if (!email) throw new Error("Email not set. Please login again.");

      const response = await fetch("http://192.168.0.13:6004/auth-service/api/auth/verify", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({ email, code })
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || "Verification failed");
      }

      const data = await response.json();

      const token = data.token;
 const decoded = jwtDecode<MyJwtPayload>(token);

const user = {
  id: decoded.UserId,
  email,
  name: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
  role: decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
  token
};

      await SecureStore.setItemAsync("auth_token", token);
      set({ user, token });

      router.push("/(tabs)");
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : "Verification failed";
      set({ error: errorMessage });
      throw err;
    } finally {
      set({ isLoading: false });
    }
  },

  logout: async () => {
    await SecureStore.deleteItemAsync("auth_token");
    set({ user: null, token: null, email: null });
    router.replace("/WelcomeScreen");
  },

  clearError: () => set({ error: null }),
}));

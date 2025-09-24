import { create } from 'zustand';
import * as SecureStore from 'expo-secure-store';
import { router } from 'expo-router';

interface Appointment {
  id: string;
  patientId: string;
  doctorId: string;
  appointmentTime: string;
  duration: string;
  location: string;
  status: 'Scheduled' | 'Confirmed' | 'Cancelled' | 'Completed' | 'NoShow';
  doctorName: string;
  doctorSpecialty: string;
  patientName: string;
  notes?: string;
  createdAt: string;
  updatedAt?: string;
}

interface AppointmentState {
  appointments: Appointment[];
  upcomingAppointments: Appointment[];
  pastAppointments: Appointment[];
  selectedAppointment: Appointment | null;
  isLoading: boolean;
  error: string | null;

    addAppointment: (appointment: Appointment) => void;
  fetchAppointments: (patientId: string) => Promise<void>;
  fetchUpcomingAppointments: (patientId: string) => Promise<void>;
  fetchPastAppointments: (patientId: string) => Promise<void>;
  getAppointmentById: (id: string) => Promise<void>;
  scheduleAppointment: (appointment: Omit<Appointment, 'id' | 'createdAt' | 'updatedAt'>) => Promise<void>;
  cancelAppointment: (id: string) => Promise<void>;
  rescheduleAppointment: (id: string, newTime: string) => Promise<void>;
  confirmAppointment: (id: string) => Promise<void>;
  clearError: () => void;
  clearSelectedAppointment: () => void;
  fetchDoctorAppointments: (doctorId: string) => Promise<void>;
}

export const useAppointmentStore = create<AppointmentState>((set, get) => ({
  appointments: [],
  upcomingAppointments: [],
  pastAppointments: [],
  selectedAppointment: null,
  isLoading: false,
  error: null,

  addAppointment: (appointment: Appointment) => set((state) => ({
  appointments: [...state.appointments, appointment],
  upcomingAppointments: [...state.upcomingAppointments, appointment]
})),

  fetchAppointments: async (patientId) => {
    set({ isLoading: true, error: null });
    try {
      const token = await SecureStore.getItemAsync('auth_token');
      if (!token) throw new Error('Not authenticated');

      const response = await fetch(`http://192.168.0.13:6003/api/appointments/patient/${patientId}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Failed to fetch appointments');
      }

      const data = await response.json();
      set({ appointments: data });
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to fetch appointments';
      set({ error: errorMessage });
      throw err;
    } finally {
      set({ isLoading: false });
    }
  },

  fetchUpcomingAppointments: async (patientId) => {
    set({ isLoading: true, error: null });
    try {
      const token = await SecureStore.getItemAsync('auth_token');
      if (!token) throw new Error('Not authenticated');

      const response = await fetch(`https://localhost:6063/api/appointments/patient/${patientId}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) throw new Error('Failed to fetch upcoming appointments');

      const data = await response.json();
      set({ upcomingAppointments: data });
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to fetch upcoming appointments';
      set({ error: errorMessage });
      throw err;
    } finally {
      set({ isLoading: false });
    }
  },

  fetchPastAppointments: async (patientId) => {
    set({ isLoading: true, error: null });
    try {
      const token = await SecureStore.getItemAsync('auth_token');
      if (!token) throw new Error('Not authenticated');

      const response = await fetch(`http://192.168.0.13:6003/api/appointments/patient/${patientId}/past`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) throw new Error('Failed to fetch past appointments');

      const data = await response.json();
      set({ pastAppointments: data });
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to fetch past appointments';
      set({ error: errorMessage });
      throw err;
    } finally {
      set({ isLoading: false });
    }
  },

  getAppointmentById: async (id) => {
    set({ isLoading: true, error: null });
    try {
      const token = await SecureStore.getItemAsync('auth_token');
      if (!token) throw new Error('Not authenticated');

      const response = await fetch(`http://192.168.0.13:6003/api/appointments/${id}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) throw new Error('Failed to fetch appointment');

      const data = await response.json();
      set({ selectedAppointment: data });
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to fetch appointment';
      set({ error: errorMessage });
      throw err;
    } finally {
      set({ isLoading: false });
    }
  },

  scheduleAppointment: async (appointment) => {
    set({ isLoading: true, error: null });
    try {
      const token = await SecureStore.getItemAsync('auth_token');
      if (!token) throw new Error('Not authenticated');

      const response = await fetch('http://192.168.0.13:6003/api/appointments', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(appointment)
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Failed to schedule appointment');
      }

      const newAppointment = await response.json();
      set((state) => ({
        appointments: [...state.appointments, newAppointment],
        upcomingAppointments: [...state.upcomingAppointments, newAppointment]
      }));

      router.push('/(tabs)/appointments');
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to schedule appointment';
      set({ error: errorMessage });
      throw err;
    } finally {
      set({ isLoading: false });
    }
  },

  cancelAppointment: async (id) => {
     console.log("Canceld: "+id)
    set({ isLoading: true, error: null });
    try {
    

      const response = await fetch(`http://192.168.0.13:6003/api/appointments/${id}/cancel`, {
        method: 'PUT',
      
      });

   
      if (!response.ok) throw new Error('Failed to cancel appointment');

      set((state) => ({
        appointments: state.appointments.map(a => 
          a.id === id ? { ...a, status: 'Cancelled' } : a
        ),
        upcomingAppointments: state.upcomingAppointments.filter(a => a.id !== id)
      }));
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to cancel appointment';
      set({ error: errorMessage });
      throw err;
    } finally {
      set({ isLoading: false });
    }
  },

  rescheduleAppointment: async (id, newTime) => {
    set({ isLoading: true, error: null });
    try {
      const token = await SecureStore.getItemAsync('auth_token');
      if (!token) throw new Error('Not authenticated');

      const response = await fetch(`http://192.168.0.13:6003/api/appointments/${id}/reschedule`, {
        method: 'PUT',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ newAppointmentTime: newTime })
      });

      console.log("Status: "+newTime.valueOf())
      if (!response.ok) throw new Error('Failed to reschedule appointment');

      const updatedAppointment = await response.json();
      set((state) => ({
        appointments: state.appointments.map(a => 
          a.id === id ? updatedAppointment : a
        ),
        upcomingAppointments: state.upcomingAppointments.map(a => 
          a.id === id ? updatedAppointment : a
        ),
        selectedAppointment: updatedAppointment
      }));
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to reschedule appointment';
      set({ error: errorMessage });
      throw err;
    } finally {
      set({ isLoading: false });
    }
  },

  confirmAppointment: async (id) => {
    set({ isLoading: true, error: null });
    try {
      const token = await SecureStore.getItemAsync('auth_token');
      if (!token) throw new Error('Not authenticated');

      const response = await fetch(`http://192.168.0.13:6003/api/appointments/${id}/confirm`, {
        method: 'PUT',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) throw new Error('Failed to confirm appointment');

      set((state) => ({
        appointments: state.appointments.map(a => 
          a.id === id ? { ...a, status: 'Confirmed' } : a
        ),
        upcomingAppointments: state.upcomingAppointments.map(a => 
          a.id === id ? { ...a, status: 'Confirmed' } : a
        ),
        selectedAppointment: state.selectedAppointment 
          ? { ...state.selectedAppointment, status: 'Confirmed' } 
          : null
      }));
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Failed to confirm appointment';
      set({ error: errorMessage });
      throw err;
    } finally {
      set({ isLoading: false });
    }
  },
  fetchDoctorAppointments: async (doctorId: string) => {
  set({ isLoading: true, error: null });
  try {
    const token = await SecureStore.getItemAsync('auth_token');
    if (!token) throw new Error('Not authenticated');

    const response = await fetch(`http://192.168.0.13:6003/api/appointments/doctor/${doctorId}`, {
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || 'Failed to fetch doctor appointments');
    }

    const data = await response.json();
    set({ appointments: data });
  } catch (err) {
    const errorMessage = err instanceof Error ? err.message : 'Failed to fetch doctor appointments';
    set({ error: errorMessage });
    throw err;
  } finally {
    set({ isLoading: false });
  }
},


  clearError: () => set({ error: null }),
  clearSelectedAppointment: () => set({ selectedAppointment: null })
}));
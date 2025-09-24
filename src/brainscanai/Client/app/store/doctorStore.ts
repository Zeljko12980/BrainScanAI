import { create } from 'zustand';
import { persist } from 'zustand/middleware';

interface Doctor {
  id: string;
  firstname: string;
  lastname: string;
  email: string;
  phoneNumber: string;
  specialization: string;
}

interface DoctorStore {
  doctor: Doctor | null;
  getDoctorById: (id: string) => Promise<void>;
  clearDoctor: () => void;
}

export const useDoctorStore = create<DoctorStore>()(
  persist(
    (set) => ({
      doctor: null,
      
      getDoctorById: async (id: string) => {
        try {
          // Replace with your actual API endpoint
          const response = await fetch(`http://192.168.0.13:6004/doctor-service/api/doctors/${id}`, {
          
          });
          
          if (!response.ok) {
            throw new Error('Failed to fetch doctor data');
          }
          
          const data: Doctor = await response.json();
          set({ doctor: data });
        } catch (error) {
          console.error('Error fetching doctor data:', error);
          set({ doctor: null });
        }
      },
      
      clearDoctor: () => set({ doctor: null }),
    }),
    {
      name: 'doctor-storage', // name for the persisted data
    }
  )
);
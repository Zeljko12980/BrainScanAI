import { create } from 'zustand';
import axios from 'axios';

interface PatientStore {
  patient: PatientDto | null;
  loading: boolean;
  error: string | null;
   patients: PatientDto[];  
  getPatientById: (id: string) => Promise<void>;
    getAllPatients: () => Promise<void>;
}

export interface MedicalEventDto {
  id: string;
  date: string;
  description: string;
  type: string;
}

export interface MedicalHistoryDto {
  events: MedicalEventDto[];
}

export interface PatientDto {
  id: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  jmbg: string;
  gender: string;
  phone: string;
  email: string;
  address: string;
  bloodType: string;
  emergencyContactName: string;
  emergencyContactPhone: string;
  emergencyContactRelation: string;
  allergies: string[];
  chronicDiseases: string[];
  medications: string[];
  scanImages: string[];
  createdAt: string;
  updatedAt?: string;
  medicalHistory: MedicalHistoryDto;
}


export const usePatientStore = create<PatientStore>((set) => ({
  patient: null,
  loading: false,
    patients: [],
  error: null,
  getPatientById: async (id: string) => {
    set({ loading: true, error: null });
    try {
      const response = await axios.get<PatientDto>(`http://192.168.0.13:6004/patient-service/api/patients/${id}`);
      set({ patient: response.data, loading: false });
    } catch (err: any) {
      set({ error: err.message, loading: false });
    }
  },
  
  getAllPatients: async () => {    // nova metoda
    set({ loading: true, error: null });
    try {
      const response = await axios.get<PatientDto[]>(
        `http://192.168.0.13:6004/patient-service/api/patients`
      );
      set({ patients: response.data, loading: false });
    } catch (err: any) {
      set({ error: err.message, loading: false });
    }
  },
}));

import { StyleSheet, ScrollView, TouchableOpacity, ActivityIndicator, Modal, TextInput } from 'react-native';
import { Text, View } from '@/components/Themed';
import { Ionicons } from '@expo/vector-icons';
import React, { useEffect, useState } from 'react';
import { useFocusEffect, useIsFocused } from '@react-navigation/native';
import { useCallback } from 'react';

import DateTimePicker from '@react-native-community/datetimepicker';
import { format, isAfter, isBefore } from 'date-fns';
import MapModal from '../(modals)/MapModal';
import { useAppointmentStore } from '../store/appoitmentStore';
import { useAuthStore } from '../store/authStore';
import { initAppointmentSocket, stopAppointmentSocket } from '../services/socketAppointmentService';

type AppointmentStatus = 'Scheduled' | 'Confirmed' | 'Cancelled' | 'Completed' | 'NoShow';
interface Appointment {
  id: string;                 
  patientId: string;          
  doctorId: string;            
  doctorName: string;           
  doctorSpecialty: string;    
  appointmentTime: string;      
  duration: string;             
  status: AppointmentStatus; 
  location: string;             
  reason?: string;              
  notes?: string;               
  createdAt: string;           
  updatedAt?: string;          
}
export default function TabTwoScreen() {
  const { 
    appointments, 
    isLoading,
    fetchAppointments,
    cancelAppointment,
    rescheduleAppointment,
    confirmAppointment,
    fetchDoctorAppointments
    
  } = useAppointmentStore();

  const [rescheduleModalVisible, setRescheduleModalVisible] = useState(false);
  const [selectedAppointmentId, setSelectedAppointmentId] = useState<string | null>(null);
  const [newAppointmentTime, setNewAppointmentTime] = useState(new Date());
  const [showDatePicker, setShowDatePicker] = useState<'date' | 'time' | null>(null);
  const [isRescheduling, setIsRescheduling] = useState(false);
  const [rescheduleError, setRescheduleError] = useState<string | null>(null);
  const [mapVisible, setMapVisible] = useState(false);
  const { user } = useAuthStore();
  const isFocused = useIsFocused();
  // Separate appointments into past and upcoming
const { upcomingAppointments, pastAppointments } = React.useMemo(() => {
  const now = new Date();
  const upcoming: Appointment[] = [];
  const past: Appointment[] = [];

  appointments?.forEach(appt => {
    if (isAfter(new Date(appt.appointmentTime), now)) {
      upcoming.push(appt);
    } else {
      past.push(appt);
    }
  });

  return { upcomingAppointments: upcoming, pastAppointments: past };
}, [appointments]);

 useEffect(() => {
    if (user?.id ) {
      const conn = initAppointmentSocket(user?.id!);

      return () => {
        stopAppointmentSocket();
      };
    }
  }, [user?.id]);

  const focaHospitalRS = {
    name: "FOCA Foča - Opšta bolnica",
    address: "Save Kovačevića bb, Foča 73300, Republika Srpska, BiH",
    latitude: 43.5083,
    longitude: 18.7786
  };

 useEffect(() => {
  if (isFocused) {
    if (user?.id && user.role === 'Patient') {
      fetchAppointments(user.id);
    } else if (user?.id) {
      fetchDoctorAppointments(user.id);
    }
  }
}, [isFocused, user?.id, user?.role]);

  useFocusEffect(
  useCallback(() => {
    if (user?.id && user.role === 'Patient') {
      fetchAppointments(user.id);
    } else if (user?.id) {
      fetchDoctorAppointments(user.id);
    }
  }, [user?.id, user?.role])
);

  const handleCancel = async (appointmentId: string) => {
    try {
      console.log("Canceld: "+appointmentId)
     await cancelAppointment(appointmentId);
    } catch (error) {
       console.log("Error: "+error)
    }
  };

  const handleReschedule = (appointmentId: string) => {
    setSelectedAppointmentId(appointmentId);
    setRescheduleModalVisible(true);
    const appointment = upcomingAppointments.find(a => a.id === appointmentId);
    if (appointment) {
      setNewAppointmentTime(new Date(appointment.appointmentTime));
    }
  };

  const handleConfirmReschedule = async () => {
    if (!selectedAppointmentId) return;
    
    setIsRescheduling(true);
    setRescheduleError(null);
    
    try {
      
      await rescheduleAppointment(selectedAppointmentId, newAppointmentTime.toISOString());

      
      setRescheduleModalVisible(false);
      if (user?.id) {
        fetchAppointments(user.id); 
      }
    } catch (err) {
      setRescheduleError(err instanceof Error ? err.message : 'Failed to reschedule appointment');
  
    } finally {
      setIsRescheduling(false);
    }
  };

  const handleConfirm = async (appointmentId: string) => {
    try {
      await confirmAppointment(appointmentId);
    } catch (error) {}
  };

  const onChangeDateTime = (event: any, selectedDate?: Date) => {
    setShowDatePicker(null);
    if (selectedDate) {
      if (showDatePicker === 'time') {
        const newTime = new Date(selectedDate);
        const updatedDate = new Date(newAppointmentTime);
        updatedDate.setHours(newTime.getHours());
        updatedDate.setMinutes(newTime.getMinutes());
        setNewAppointmentTime(updatedDate);
      } else {
        setNewAppointmentTime(selectedDate);
      }
    }
  };

  if (isLoading && !appointments.length) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" color="#4a6cf7" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
        <View style={styles.header}>
          <Text style={styles.title}>My Appointments</Text>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Upcoming Appointments</Text>
          {upcomingAppointments.length > 0 ? (
            upcomingAppointments.map((appointment) => (
              <View key={appointment.id} style={styles.card}>
                <View style={styles.cardHeader}>
                  <Ionicons name="calendar" size={24} color="#4a6cf7" />
                  <Text style={styles.cardTitle}>
                    {format(new Date(appointment.appointmentTime), "MMMM d, yyyy 'at' h:mm a")}
                  </Text>
                  <View style={[
                    styles.statusBadge,
                    appointment.status === 'Confirmed' 
                      ? styles.confirmedStatus 
                      : appointment.status === 'Cancelled'
                      ? styles.cancelledStatus
                      : styles.pendingStatus
                  ]}>
                    <Text style={styles.statusText}>{appointment.status}</Text>
                  </View>
                </View>
                
                <View style={styles.infoRow}>
                  <Text style={styles.infoLabel}>Doctor:</Text>
                  <Text style={styles.infoValue}>{appointment.doctorName}</Text>
                </View>
                <View style={styles.infoRow}>
                  <Text style={styles.infoLabel}>Specialty:</Text>
                  <Text style={styles.infoValue}>{appointment.doctorSpecialty}</Text>
                </View>
                <View style={styles.infoRow}>
                  <Text style={styles.infoLabel}>Location:</Text>
                  <Text style={styles.infoValue}>{appointment.location}</Text>
                </View>

                <View style={styles.actionsRow}>
                  {appointment.status === 'Scheduled' && (
                    <>
                      <TouchableOpacity 
                        style={styles.actionButton}
                        onPress={() => handleReschedule(appointment.id)}
                      >
                        <Text style={styles.actionButtonText}>Reschedule</Text>
                      </TouchableOpacity>
                      <TouchableOpacity 
                        style={styles.actionButton}
                        onPress={() => handleCancel(appointment.id)}
                      >
                        <Text style={styles.actionButtonText}>Cancel</Text>
                      </TouchableOpacity>
                      <TouchableOpacity 
                        style={styles.actionButtonPrimary}
                        onPress={() => handleConfirm(appointment.id)}
                      >
                        <Text style={styles.actionButtonPrimaryText}>Confirm</Text>
                      </TouchableOpacity>
                    </>
                  )}
                  {appointment.status === 'Confirmed' && (
                    <>
                      <TouchableOpacity 
                        style={styles.actionButtonPrimary}
                        onPress={() => setMapVisible(true)}
                      >
                        <Text style={styles.actionButtonPrimaryText}>View on Map</Text>
                      </TouchableOpacity>

                      <MapModal
                        visible={mapVisible}
                        onClose={() => setMapVisible(false)}
                        location={focaHospitalRS.address}
                        latitude={focaHospitalRS.latitude}
                        longitude={focaHospitalRS.longitude}
                      />
                    </>
                  )}
                </View>
              </View>
            ))
          ) : (
            <View style={styles.emptyState}>
              <Ionicons name="calendar-outline" size={48} color="#cccccc" />
              <Text style={styles.emptyStateText}>No upcoming appointments</Text>
            </View>
          )}
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Past Appointments</Text>
          {pastAppointments.length > 0 ? (
            pastAppointments.map((appointment) => (
              <View key={appointment.id} style={styles.card}>
                <View style={styles.cardHeader}>
                  <Ionicons name="checkmark-circle" size={24} color="#4a6cf7" />
                  <Text style={styles.cardTitle}>
                    {format(new Date(appointment.appointmentTime), "MMMM d, yyyy 'at' h:mm a")}
                  </Text>
                </View>
                <View style={styles.infoRow}>
                  <Text style={styles.infoLabel}>Doctor:</Text>
                  <Text style={styles.infoValue}>{appointment.doctorName}</Text>
                </View>
                <View style={styles.infoRow}>
                  <Text style={styles.infoLabel}>Specialty:</Text>
                  <Text style={styles.infoValue}>{appointment.doctorSpecialty}</Text>
                </View>
                <View style={styles.infoRow}>
                  <Text style={styles.infoLabel}>Status:</Text>
                  <Text style={styles.infoValue}>{appointment.status}</Text>
                </View>
                <TouchableOpacity style={styles.viewDetailsButton}>
                  <Text style={styles.viewDetailsText}>View Visit Details</Text>
                  <Ionicons name="chevron-forward" size={16} color="#4a6cf7" />
                </TouchableOpacity>
              </View>
            ))
          ) : (
            <View style={styles.emptyState}>
              <Ionicons name="time-outline" size={48} color="#cccccc" />
              <Text style={styles.emptyStateText}>No past appointments found</Text>
            </View>
          )}
        </View>
      </ScrollView>

      <Modal
        animationType="slide"
        transparent={true}
        visible={rescheduleModalVisible}
        onRequestClose={() => setRescheduleModalVisible(false)}
      >
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>Reschedule Appointment</Text>
            
            {/* Date Picker Section */}
            <View style={styles.pickerSection}>
              <Text style={styles.pickerLabel}>Select Date:</Text>
              <TouchableOpacity 
                style={styles.dateInput}
                onPress={() => setShowDatePicker('date')}
              >
                <Text style={styles.dateText}>{format(newAppointmentTime, "MMMM d, yyyy")}</Text>
                <Ionicons name="calendar" size={20} color="#4a6cf7" />
              </TouchableOpacity>
            </View>

            {/* Time Picker Section */}
            <View style={styles.pickerSection}>
              <Text style={styles.pickerLabel}>Select Time:</Text>
              <TouchableOpacity 
                style={styles.dateInput}
                onPress={() => setShowDatePicker('time')}
              >
                <Text style={styles.dateText}>{format(newAppointmentTime, "h:mm a")}</Text>
                <Ionicons name="time" size={20} color="#4a6cf7" />
              </TouchableOpacity>
            </View>

            {showDatePicker && (
              <DateTimePicker
                value={newAppointmentTime}
                mode={showDatePicker}
                display="spinner"
                onChange={onChangeDateTime}
                minimumDate={new Date()}
              />
            )}

            {rescheduleError && (
              <Text style={styles.errorText}>{rescheduleError}</Text>
            )}

            <View style={styles.modalButtonRow}>
              <TouchableOpacity 
                style={[styles.modalButton, styles.cancelButton]}
                onPress={() => setRescheduleModalVisible(false)}
                disabled={isRescheduling}
              >
                <Text style={styles.cancelButtonText}>Cancel</Text>
              </TouchableOpacity>
              <TouchableOpacity 
                style={styles.modalButton}
                onPress={handleConfirmReschedule}
                disabled={isRescheduling}
              >
                {isRescheduling ? (
                  <ActivityIndicator color="#ffffff" />
                ) : (
                  <Text style={styles.modalButtonText}>Confirm</Text>
                )}
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </Modal>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#ffffff',
  },
  scrollContent: {
    padding: 20,
    paddingBottom: 40,
  },
  header: {
    marginBottom: 24,
    backgroundColor: '#ffffff',
    marginTop: 20
  },
  title: {
    fontSize: 26,
    fontWeight: '800',
    color: '#000000',
    marginBottom: 8,
    backgroundColor: '#ffffff',
  },
  section: {
    marginBottom: 24,
    backgroundColor: '#ffffff',
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: '700',
    color: '#000000',
    marginBottom: 16,
    backgroundColor: '#ffffff',
  },
  card: {
    backgroundColor: '#ffffff',
    borderRadius: 16,
    padding: 20,
    borderWidth: 1,
    borderColor: '#f0f0f0',
    marginBottom: 16,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.05,
    shadowRadius: 4,
    elevation: 2,
  },
  cardHeader: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 16,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
    paddingBottom: 12,
    backgroundColor: '#ffffff',
  },
  cardTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: '#000000',
    marginLeft: 8,
    flex: 1,
    backgroundColor: '#ffffff',
  },
  infoRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 12,
    paddingHorizontal: 4,
    backgroundColor: '#ffffff',
  },
  infoLabel: {
    fontSize: 15,
    color: '#666666',
    fontWeight: '600',
    backgroundColor: '#ffffff',
  },
  infoValue: {
    fontSize: 15,
    color: '#333333',
    fontWeight: '500',
    backgroundColor: '#ffffff',
  },
  statusBadge: {
    paddingHorizontal: 10,
    paddingVertical: 4,
    borderRadius: 12,
  },
  confirmedStatus: {
    backgroundColor: 'rgba(74, 223, 134, 0.2)',
  },
  pendingStatus: {
    backgroundColor: 'rgba(255, 193, 7, 0.2)',
  },
  cancelledStatus: {
    backgroundColor: 'rgba(255, 59, 48, 0.2)',
  },
  statusText: {
    fontSize: 12,
    fontWeight: '600',
  },
  actionsRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 16,
    backgroundColor: '#ffffff',
  },
  actionButton: {
    paddingVertical: 10,
    paddingHorizontal: 16,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: '#e0e0e0',
    backgroundColor: '#ffffff',
  },
  actionButtonText: {
    color: '#666666',
    fontWeight: '600',
    fontSize: 14,
  },
  actionButtonPrimary: {
    paddingVertical: 10,
    paddingHorizontal: 16,
    borderRadius: 8,
    backgroundColor: 'rgba(74, 108, 247, 0.1)',
  },
  actionButtonPrimaryText: {
    color: '#4a6cf7',
    fontWeight: '600',
    fontSize: 14,
  },
  viewDetailsButton: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'flex-end',
    marginTop: 12,
    backgroundColor: '#ffffff',
  },
  viewDetailsText: {
    color: '#4a6cf7',
    fontWeight: '600',
    fontSize: 14,
    marginRight: 4,
    backgroundColor: '#ffffff',
  },
  loadingContainer: {
    flex: 1,
    backgroundColor:'transparent',
    justifyContent: 'center',
    alignItems: 'center',
  },
  emptyState: {
    alignItems: 'center',
    justifyContent: 'center',
    padding: 32,
    backgroundColor: '#ffffff',
  },
  emptyStateText: {
    marginTop: 16,
    fontSize: 16,
    color: '#666666',
    textAlign: 'center',
    backgroundColor: '#ffffff',
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
    padding: 20,
  },
  modalContent: {
    backgroundColor: '#fff',
    borderRadius: 16,
    padding: 20,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 8,
    elevation: 5,
  },
  modalTitle: {
    fontSize: 20,
    fontWeight: 'bold',
    marginBottom: 20,
    textAlign: 'center',
    color: '#000', 
    backgroundColor: '#fff',
  },
  pickerSection: {
    marginBottom: 20,
    backgroundColor: '#fff',
  },
  pickerLabel: {
    fontSize: 16,
    fontWeight: 'bold',
    marginBottom: 8,
    color: '#000',
    backgroundColor: '#fff',
  },
  dateInput: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    backgroundColor: '#fff',
    alignItems: 'center',
    color:'#000',
    padding: 15,
    borderWidth: 1,
    borderColor: '#e0e0e0',
    borderRadius: 8,
    marginBottom: 20,
  },
  dateInputText: { 
    color: '#000', 
    fontWeight: 'bold',
    fontSize: 16,
  },
  modalButton: {
    backgroundColor: '#4a6cf7',
    padding: 15,
    borderRadius: 8,
    alignItems: 'center',
    flex: 1,
    marginHorizontal: 5,
  },
  cancelButton: {
    backgroundColor: '#f0f0f0',
  },
  modalButtonText: {
    color: '#fff',
    fontWeight: 'bold', 
  },
  cancelButtonText: {
    color: '#000', 
    fontWeight: 'bold', 
  },
  modalButtonRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    backgroundColor: '#fff',
    marginTop: 20,
  },
  errorText: {
    color: 'red',
    marginBottom: 15,
    textAlign: 'center',
    backgroundColor: '#fff',
    fontWeight: 'bold', 
  },
  pickerContainer: {
    backgroundColor: '#fff',
    borderRadius: 8,
    marginTop: 10,
  },
  dateText: {
    color: '#000000', 
    fontWeight: '600',
    fontSize: 16,
  },
});
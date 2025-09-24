import { StyleSheet, Modal, TouchableOpacity, ScrollView } from 'react-native';
import { Text, View } from '@/components/Themed';

import { Redirect } from 'expo-router';
import { ActivityIndicator } from 'react-native';
import React, { useEffect, useState } from 'react';
import { Ionicons } from '@expo/vector-icons';
import { Image } from 'react-native';

import { router } from 'expo-router';
import { jwtDecode } from 'jwt-decode';
import { useAppointmentStore } from '../store/appoitmentStore';
import { useAuthStore, MyJwtPayload } from '../store/authStore';
import { useNotificationStore } from '../store/notificationStore';
export default function TabOneScreen() {
  const { user, isLoading: isAuthLoading,token } = useAuthStore();
  const [modalVisible, setModalVisible] = useState(true);
const [isAppointmentModalVisible, setIsAppointmentModalVisible] = useState(false);

const [newAppointment, setNewAppointment] = useState({
  doctorId: '',
  appointmentTime: '',
  duration: '30', 
  location: '',
  notes: ''
});
  const { 
    appointments,
    isLoading: isAppointmentsLoading,
    fetchAppointments,
    scheduleAppointment,
  } = useAppointmentStore();

  const{
    notifications,
    fetchNotifications
  }=useNotificationStore();



   const nearestAppointment = appointments.length > 0 
    ? appointments[0] // Assuming they're sorted by date
    : null;

  useEffect(() => {
    if (user?.id) {
      fetchAppointments(user.id);
      fetchNotifications(user.id)
    }
  }, [user?.id]);

  const handleScheduleAppointment = async () => {
  try {
    await scheduleAppointment({
      patientId: user?.id || '',
      doctorId: newAppointment.doctorId,
      appointmentTime: newAppointment.appointmentTime,
      duration: newAppointment.duration,
      location: newAppointment.location,
      status: 'Scheduled',
      doctorName: '', // You'll need to fetch this
      doctorSpecialty: '', // You'll need to fetch this
      patientName: `${user?.name}`,
      notes: newAppointment.notes
    });
    setIsAppointmentModalVisible(false);
    // Reset form
    setNewAppointment({
      doctorId: '',
      appointmentTime: '',
      duration: '30',
      location: '',
      notes: ''
    });
  } catch (error) {
    console.error('Error scheduling appointment:', error);
  }
};

  if (isAuthLoading || isAppointmentsLoading) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" color="#4a6cf7" />
      </View>
    );
  }



  if (!user) {
    return <Redirect href="/(screen)/WelcomeScreen" />;
  }

  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
     
<View style={styles.header}>
 <Image
  source={{
    uri:'https://randomuser.me/api/portraits/men/1.jpg'
  }}
  style={styles.userImage}
/>
  <Text style={styles.title}>Welcome Back, {user.name}</Text>
  <Text style={styles.subtitle}>Medical Dashboard</Text>
</View>

        {/* Quick Actions */}
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Quick Actions</Text>
          <View style={styles.actionsRow}>
            <View style={styles.actionButton}>
            <TouchableOpacity 
  style={styles.actionIconContainer}
  onPress={() => router.push('/(tabs)/appointments')}
>
  <Ionicons name="calendar" size={24} color="#4a6cf7" />
</TouchableOpacity>
              <Text style={styles.actionLabel}>Book Visit</Text>
            </View>
            <View style={styles.actionButton}>
                       <TouchableOpacity 
  style={styles.actionIconContainer}
  onPress={() => router.push('/(tabs)/appointments')}
>
                <Ionicons name="time" size={24} color="#4a6cf7" />
             </TouchableOpacity>
              <Text style={styles.actionLabel}>My Schedule</Text>
            </View>
            <View style={styles.actionButton}>
                <TouchableOpacity 
  style={styles.actionIconContainer}
  onPress={() => router.push('/(tabs)/records')}
>
                <Ionicons name="document-text" size={24} color="#4a6cf7" />
              </TouchableOpacity>
              <Text style={styles.actionLabel}>Records</Text>
            </View>
          </View>
        </View>

        {/* Medical Card */}
    <View style={styles.section}>
          <Text style={styles.sectionTitle}>Upcoming Appointment</Text>
          
          {nearestAppointment ? (
            <View style={styles.card}>
              <View style={styles.cardHeader}>
                <Ionicons name="medical" size={24} color="#4a6cf7" />
                <Text style={styles.cardTitle}>Your Next Visit</Text>
                <View style={[
                  styles.statusBadge,
                  nearestAppointment.status === 'Confirmed' ? styles.confirmedStatus : 
                  nearestAppointment.status === 'Cancelled' ? styles.cancelledStatus :
                  styles.pendingStatus
                ]}>
                  <Text style={styles.statusText}>{nearestAppointment.status}</Text>
                </View>
              </View>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Doctor:</Text>
                <Text style={styles.infoValue}>{nearestAppointment.doctorName}</Text>
              </View>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Specialty:</Text>
                <Text style={styles.infoValue}>{nearestAppointment.doctorSpecialty}</Text>
              </View>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Date:</Text>
                <Text style={styles.infoValue}>
                  {new Date(nearestAppointment.appointmentTime).toLocaleDateString()} •{' '}
                  {new Date(nearestAppointment.appointmentTime).toLocaleTimeString([], { 
                    hour: '2-digit', 
                    minute: '2-digit' 
                  })}
                </Text>
              </View>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Location:</Text>
                <Text style={styles.infoValue}>{nearestAppointment.location}</Text>
              </View>
            </View>
          ) : (
            <View style={styles.emptyState}>
              <Ionicons name="calendar-outline" size={48} color="#cccccc" />
              <Text style={styles.emptyStateText}>No upcoming appointments scheduled</Text>
            </View>
          )}
        </View>

        {/* Notifications */}
    <View style={styles.section}>
  <View style={styles.sectionHeader}>
    <Text style={styles.sectionTitle}>Recent Notifications</Text>
    <TouchableOpacity onPress={() => router.push("/(tabs)/messages")}>
      <Text style={styles.seeAll}>View All</Text>
    </TouchableOpacity>
  </View>

  {notifications.slice(0, 2).map((notification) => (
    <View key={notification.id} style={styles.notificationItem}>
      <View style={styles.notificationIcon}>
        <Ionicons 
          name={notification.type === 'alert' ? "alert-circle" : "notifications"} 
          size={20} 
          color="#4a6cf7" 
        />
      </View>
      <View style={styles.notificationTextContainer}>
        <Text style={styles.notificationText}>{notification.message}</Text>
        <Text style={styles.notificationTime}>
          {new Date(notification.createdAt).toLocaleString()}
        </Text>
      </View>
    </View>
  ))}

 {notifications.length === 0 && (
  <View style={{ padding: 16, alignItems: 'center', backgroundColor: '#ffffff' }}>
    <Text style={{ color: '#000000' }}>No notifications found.</Text>
  </View>
)}

</View>
      </ScrollView>

      {/* Welcome Modal */}
      <Modal
        animationType="slide"
        transparent={true}
        visible={modalVisible}
        onRequestClose={() => setModalVisible(false)}
      >
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <Ionicons name="checkmark-circle" size={64} color="#4a6cf7" style={styles.modalIcon} />
            <Text style={styles.modalTitle}>Welcome Back!</Text>
            <Text style={styles.modalText}>You're successfully logged in as {user.name}</Text>
            <TouchableOpacity 
              style={styles.modalButton}
              onPress={() => setModalVisible(false)}
            >
              <Text style={styles.modalButtonText}>Continue to Dashboard</Text>
            </TouchableOpacity>
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
    paddingBottom: 100,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#ffffff',
  },
  header: {
    alignItems: 'center',
    marginTop:20,
    marginBottom: 30,
    backgroundColor: '#ffffff',
  },
  iconContainer: {
    backgroundColor: 'rgba(74, 108, 247, 0.1)',
    width: 80,
    height: 80,
    borderRadius: 40,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 16,
  },
  title: {
    fontSize: 26,
    fontWeight: '800',
    color: '#000000',
    marginBottom: 8,
    textAlign: 'center',
    backgroundColor: '#ffffff',
  },
  subtitle: {
    fontSize: 16,
    color: '#666666',
    fontWeight: '500',
    textAlign: 'center',
    backgroundColor: '#ffffff',
  },
  section: {
    marginBottom: 24,
    backgroundColor: '#ffffff',
  },
  sectionHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
    backgroundColor: '#ffffff',
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: '700',
    color: '#000000',
    backgroundColor: '#ffffff',
  },
  seeAll: {
    color: '#4a6cf7',
    fontSize: 14,
    fontWeight: '600',
  },
  actionsRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 12,
    backgroundColor: '#ffffff',
  },
  actionButton: {
    alignItems: 'center',
    width: '30%',
    backgroundColor: '#ffffff',
  },
  actionIconContainer: {
    backgroundColor: 'rgba(74, 108, 247, 0.1)',
    width: 50,
    height: 50,
    borderRadius: 25,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 8,
  },
  actionLabel: {
    fontSize: 12,
    fontWeight: '600',
    color: '#4a6cf7',
    textAlign: 'center',
  },
  card: {
    backgroundColor: '#ffffff',
    borderRadius: 16,
    padding: 20,
    borderWidth: 1,
    borderColor: '#f0f0f0',
    marginBottom: 24,
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
    fontSize: 18,
    fontWeight: '700',
    color: '#000000',
    marginLeft: 8,
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
  notificationItem: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#ffffff',
    borderRadius: 12,
    padding: 16,
    marginBottom: 10,
    borderWidth: 1,
    borderColor: '#f0f0f0',
  },
  notificationIcon: {
    marginRight: 12,
  },
  notificationTextContainer: {
    flex: 1,
    backgroundColor: '#ffffff',
  },
  notificationText: {
    fontSize: 14,
    color: '#333333',
    fontWeight: '500',
    backgroundColor: '#ffffff',
  },
  notificationTime: {
    fontSize: 12,
    color: '#999999',
    marginTop: 4,
    backgroundColor: '#ffffff',
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    backgroundColor: 'rgba(255,255,255,0.9)',
  },
  modalContent: {
    backgroundColor: '#ffffff',
    borderRadius: 20,
    padding: 30,
    margin: 20,
    alignItems: 'center',
    borderWidth: 1,
    borderColor: '#f0f0f0',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 4 },
    shadowOpacity: 0.1,
    shadowRadius: 10,
    elevation: 5,
  },
  modalIcon: {
    marginBottom: 20,
  },
  modalTitle: {
    fontSize: 22,
    fontWeight: '700',
    color: '#000000',
    marginBottom: 8,
    textAlign: 'center',
    backgroundColor: '#ffffff',
  },
  modalText: {
    fontSize: 16,
    color: '#666666',
    textAlign: 'center',
    marginBottom: 24,
    backgroundColor: '#ffffff',
  },
  modalButton: {
    backgroundColor: '#4a6cf7',
    paddingVertical: 14,
    paddingHorizontal: 24,
    borderRadius: 12,
    width: '100%',
    alignItems: 'center',
  },
  modalButtonText: {
    color: '#ffffff',
    fontSize: 16,
    fontWeight: '600',
  },
  userImage: {
  width: 80,
  height: 80,
  borderRadius: 40,
  marginBottom: 16,
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
    backgroundColor: 'rgba(255, 71, 87, 0.2)',
  },
  statusText: {
    fontSize: 12,
    fontWeight: '600',
  },
  emptyState: {
    alignItems: 'center',
    justifyContent: 'center',
    padding: 32,
    backgroundColor: '#ffffff',
    borderRadius: 16,
    borderWidth: 1,
    borderColor: '#f0f0f0',
  },
  emptyStateText: {
    marginTop: 16,
    fontSize: 16,
    color: '#666666',
    textAlign: 'center',
    backgroundColor: '#ffffff',
  },
});
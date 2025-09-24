import { StyleSheet, ScrollView, TouchableOpacity, Switch, Modal, Image, Linking } from 'react-native';
import { Text, View } from '@/components/Themed';

import { Redirect } from 'expo-router';
import { ActivityIndicator } from 'react-native';
import React, { useState, useEffect } from 'react';
import { Ionicons } from '@expo/vector-icons';
import { jwtDecode } from 'jwt-decode';
import { useAuthStore, MyJwtPayload } from '../store/authStore';
import { useDoctorStore } from '../store/doctorStore';
import { usePatientStore } from '../store/patientStore';


export default function ProfileScreen() {
  const { user, isLoading, logout,token } = useAuthStore();
  const { patient, getPatientById } = usePatientStore(); // Get patient store methods
  const [notificationsEnabled, setNotificationsEnabled] = useState(true);
  const [darkModeEnabled, setDarkModeEnabled] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);
  const [loadingPatient, setLoadingPatient] = useState(false);
const decodedToken = jwtDecode<MyJwtPayload>(token!);
const profileImage = decodedToken.ProfileImage;
  const { doctor, getDoctorById } = useDoctorStore();
  useEffect(() => {
    if (user?.role === 'Patient' && user?.id) {
      setLoadingPatient(true);
      getPatientById(user.id)
        .finally(() => setLoadingPatient(false));
    }
    else{
        setLoadingPatient(true);
      getDoctorById(user?.id!)
        .finally(() =>setLoadingPatient(false));
    }
  }, [user?.id]);

  if (isLoading || loadingPatient) {
    return (
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="large" color="#4a6cf7" />
      </View>
    );
  }

  if (!user) {
    return <Redirect href="/(screen)/WelcomeScreen" />;
  }

  // Use actual patient data if available
  const patientData = user.role === 'Patient' && patient ? {
    fullName: {
      firstName: patient.firstName,
      lastName: patient.lastName
    },
    dateOfBirth: patient.dateOfBirth ? new Date(patient.dateOfBirth) : new Date(),
    jmbg: patient.jmbg || "Not specified",
    gender: patient.gender || "Not specified",
    contact: {
      email: patient.email || user.email || "Not specified",
      phone: patient.phone || "Not specified",
      address: patient.address || "Not specified"
    },
    bloodType: patient.bloodType || "Not specified",
    emergencyContact: {
      fullName: patient.emergencyContactName || "Not specified",
      phone: patient.emergencyContactPhone || "Not specified",
      relationship: patient.emergencyContactRelation || "Not specified"
    },
    allergies: patient.allergies?.length ? patient.allergies : ["None recorded"],
    chronicDiseases: patient.chronicDiseases?.length ? patient.chronicDiseases : ["None recorded"],
    medications: patient.medications?.length ? patient.medications : ["None recorded"]
  } : {
    // Default data for non-patient users
    fullName: {
      firstName:  "User",
      lastName:  ""
    },
    dateOfBirth: new Date(),
    jmbg: "Not applicable",
    gender: "Not specified",
    contact: {
      email: user.email || "Not specified",
      phone: "Not specified",
      address: "Not specified"
    },
    bloodType: "Not applicable",
    emergencyContact: {
      fullName: "Not specified",
      phone: "Not specified",
      relationship: "Not specified"
    },
    allergies: ["Not applicable"],
    chronicDiseases: ["Not applicable"],
    medications: ["Not applicable"]
  };

  return (
    <ScrollView style={styles.container} contentContainerStyle={styles.scrollContent}>
      {/* Profile Header */}
      <View style={styles.profileHeader}>
        <View style={styles.avatarContainer}>
         <Image
  source={{
    uri: profileImage 
      ? `data:image/jpeg;base64,${profileImage}` 
      : 'https://randomuser.me/api/portraits/men/1.jpg'
  }}
  style={styles.profileImage}
/>
        </View>
        <Text style={styles.profileName}>{`${patientData.fullName.firstName} ${patientData.fullName.lastName}`}</Text>
        <Text style={styles.profileEmail}>{patientData.contact.email}</Text>
        <Text style={styles.profileRole}>{user.role}</Text>
      </View>

      {/* Only show patient-specific sections if user is a patient */}
      {user.role === 'Patient' && (
        <>
          {/* Personal Information Section */}
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>Personal Information</Text>
            <View style={styles.infoCard}>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Full Name</Text>
                <Text style={styles.infoValue}>{`${patientData.fullName.firstName} ${patientData.fullName.lastName}`}</Text>
              </View>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>JMBG</Text>
                <Text style={styles.infoValue}>{patientData.jmbg}</Text>
              </View>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Date of Birth</Text>
                <Text style={styles.infoValue}>
                  {patientData.dateOfBirth.toLocaleDateString()}
                </Text>
              </View>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Gender</Text>
                <Text style={styles.infoValue}>{patientData.gender}</Text>
              </View>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Blood Type</Text>
                <Text style={styles.infoValue}>{patientData.bloodType}</Text>
              </View>
            
            </View>
          </View>

          {/* Contact Information Section */}
   <View style={styles.section}>
  <Text style={styles.sectionTitle}>Contact Information</Text>
  <View style={styles.infoCard}>
    {/* Email */}
    <View style={styles.infoRow}>
      <Text style={styles.infoLabel}>Email:</Text>
      <Text style={styles.infoValue}>{patientData.contact.email}</Text>
    </View>

    {/* Phone */}
    <View style={styles.infoRow}>
      <Text style={styles.infoLabel}>Phone:</Text>
      <Text style={styles.infoValue}>{patientData.contact.phone}</Text>
    </View>

    {/* Address */}                                                                                     
    <View style={styles.infoRow}>
      <Text style={styles.infoLabel}>Address:</Text>
      <Text style={styles.infoValue}>{patientData.contact.address}</Text>
    </View>
  </View>
</View>                                                                                                                                                                                                                        

          {/* Emergency Contact Section */}
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>Emergency Contact</Text>
            <View style={styles.infoCard}>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Name</Text>
                <Text style={styles.infoValue}>{patientData.emergencyContact.fullName}</Text>
              </View>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Phone</Text>
                <Text style={styles.infoValue}>{patientData.emergencyContact.phone}</Text>
              </View>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Relationship</Text>
                <Text style={styles.infoValue}>{patientData.emergencyContact.relationship}</Text>
              </View>
            </View>
          </View>

          {/* Medical Information Section */}
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>Medical Information</Text>
            <View style={styles.infoCard}>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Allergies</Text>
                <Text style={styles.infoValue}>{patientData.allergies.join(', ')}</Text>
              </View>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Chronic Diseases</Text>
                <Text style={styles.infoValue}>{patientData.chronicDiseases.join(', ')}</Text>
              </View>
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>Medications</Text>
                <Text style={styles.infoValue}>{patientData.medications.join(', ')}</Text>
              </View>
            </View>
          </View>
        </>
      )}
  {user.role === 'Doctor' && doctor && (
    <View style={styles.section}>
      <Text style={styles.sectionTitle}>Professional Information</Text>
      <View style={styles.infoCard}>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Specialization</Text>
          <Text style={styles.infoValue}>{doctor.specialization}</Text>
        </View>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Phone</Text>
          <Text style={styles.infoValue}>{doctor.phoneNumber}</Text>
        </View>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Email</Text>
          <Text style={styles.infoValue}>{doctor.email}</Text>
        </View>
      </View>
    </View>
  )}
      {/* Settings Section (shown for all users) */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Settings</Text>
        <View style={styles.settingsCard}>
          <View style={styles.settingItem}>
            <View style={styles.settingLeft}>
              <Ionicons name="notifications" size={24} color="#4a6cf7" />
              <Text style={styles.settingLabel}>Notifications</Text>
            </View>
            <Switch
              value={notificationsEnabled}
              onValueChange={setNotificationsEnabled}
              trackColor={{ false: "#767577", true: "#4a6cf7" }}
              thumbColor={notificationsEnabled ? "#f4f3f4" : "#f4f3f4"}
            />
          </View>

          <View style={styles.settingItem}>
            <View style={styles.settingLeft}>
              <Ionicons name="moon" size={24} color="#4a6cf7" />
              <Text style={styles.settingLabel}>Dark Mode</Text>
            </View>
            <Switch
              value={darkModeEnabled}
              onValueChange={setDarkModeEnabled}
              trackColor={{ false: "#767577", true: "#4a6cf7" }}
              thumbColor={darkModeEnabled ? "#f4f3f4" : "#f4f3f4"}
            />
          </View>

          <View style={styles.settingItem}>
            <View style={styles.settingLeft}>
              <Ionicons name="lock-closed" size={24} color="#4a6cf7" />
              <Text style={styles.settingLabel}>Change Password</Text>
            </View>
            <Ionicons name="chevron-forward" size={20} color="#666" />
          </View>

          <View style={styles.settingItem}>
            <View style={styles.settingLeft}>
              <Ionicons name="language" size={24} color="#4a6cf7" />
              <Text style={styles.settingLabel}>Language</Text>
            </View>
            <View style={styles.languageSelection}>
              <Text style={styles.languageText}>English</Text>
              <Ionicons name="chevron-forward" size={20} color="#666" />
            </View>
          </View>
        </View>
      </View>

      {/* Account Actions (shown for all users) */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Account</Text>
        <View style={styles.actionsCard}>
          <TouchableOpacity style={styles.actionItem}>
            <View style={styles.actionLeft}>
              <Ionicons name="help-circle" size={24} color="#4a6cf7" />
              <Text style={styles.actionLabel}>Help & Support</Text>
            </View>
            <Ionicons name="chevron-forward" size={20} color="#666" />
          </TouchableOpacity>

          <TouchableOpacity style={styles.actionItem}>
            <View style={styles.actionLeft}>
              <Ionicons name="information-circle" size={24} color="#4a6cf7" />
              <Text style={styles.actionLabel}>About App</Text>
            </View>
            <Ionicons name="chevron-forward" size={20} color="#666" />
          </TouchableOpacity>

          <TouchableOpacity 
            style={styles.actionItem}
            onPress={() => setModalVisible(true)}
          >
            <View style={styles.actionLeft}>
              <Ionicons name="log-out" size={24} color="#e74c3c" />
              <Text style={[styles.actionLabel, styles.logoutText]}>Log Out</Text>
            </View>
            <Ionicons name="chevron-forward" size={20} color="#666" />
          </TouchableOpacity>
        </View>
      </View>

      {/* Logout Confirmation Modal */}
    {/* Logout Confirmation Modal */}
<Modal
  animationType="fade"
  transparent={true}
  visible={modalVisible}
  onRequestClose={() => setModalVisible(false)}
>
  <View style={styles.modalOverlay}>
    <View style={styles.modalContent}>
      <Text style={styles.modalTitle}>Log Out</Text>
      <Text style={styles.modalText}>Are you sure you want to log out?</Text>
      <View style={styles.modalButtons}>
        <TouchableOpacity 
          style={[styles.modalButton, styles.cancelButton]}
          onPress={() => setModalVisible(false)}
        >
          <Text style={styles.cancelButtonText}>Cancel</Text>
        </TouchableOpacity>
        <TouchableOpacity 
          style={[styles.modalButton, styles.logoutButton]}
          onPress={logout}
        >
          <Text style={styles.logoutButtonText}>Log Out</Text>
        </TouchableOpacity>
      </View>
    </View>
  </View>
</Modal>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f9f9f9',
  },
  scrollContent: {
    paddingBottom: 40,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#f9f9f9',
  },
  profileHeader: {
    alignItems: 'center',
    paddingVertical: 30,
    backgroundColor: '#ffffff',
    marginBottom: 20,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
    marginTop:20
  },
  avatarContainer: {
    width: 120,
    height: 120,
    borderRadius: 60,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 16,
    overflow: 'hidden',
    backgroundColor: 'rgba(74, 108, 247, 0.1)',
  },
  profileImage: {
    width: '100%',
    height: '100%',
    resizeMode: 'cover',
  },
  profileName: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#1a1a1a',
    marginBottom: 4,
  },
  profileEmail: {
    fontSize: 16,
    color: '#666',
    marginBottom: 4,
  },
  profileRole: {
    fontSize: 14,
    color: '#4a6cf7',
    fontWeight: '600',
    textTransform: 'capitalize',
  },
  section: {
    marginBottom: 20,
    paddingHorizontal: 20,
    backgroundColor: 'transparent',
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#1a1a1a',
    marginBottom: 12,
  },
  infoCard: {
    backgroundColor: '#ffffff',
    borderRadius: 12,
    padding: 20,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.05,
    shadowRadius: 2,
    elevation: 1,
  },
  infoRow: {
   flexDirection: 'row',
  justifyContent: 'space-between',
  marginBottom: 12,
  paddingHorizontal: 4,
  backgroundColor:'#fff'
  },
  infoLabel: {
    fontSize: 15,
    color: '#666',
    fontWeight: '500',
  },
  infoValue: {
    fontSize: 15,
    color: '#1a1a1a',
    fontWeight: '500',
    maxWidth: '60%',
    textAlign: 'right',
  },
  editButton: {
    marginTop: 10,
    alignSelf: 'flex-end',
  },
  editButtonText: {
    color: '#4a6cf7',
    fontWeight: '600',
    fontSize: 14,
  },
  settingsCard: {
    backgroundColor: '#ffffff',
    borderRadius: 12,
    paddingVertical: 10,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.05,
    shadowRadius: 2,
    elevation: 1,
  },
  settingItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 14,
    paddingHorizontal: 16,
    backgroundColor: '#ffffff',
    borderBottomWidth: 1,
    borderBottomColor: '#f5f5f5',
  },
  settingLeft: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#ffffff',
  },
  settingLabel: {
    fontSize: 15,
    color: '#1a1a1a',
    marginLeft: 12,
    backgroundColor: '#ffffff',
  },
  languageSelection: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#ffffff',
  },
  languageText: {
    fontSize: 15,
    color: '#666',
    marginRight: 4,
  },
  actionsCard: {
    backgroundColor: '#ffffff',
    borderRadius: 12,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.05,
    shadowRadius: 2,
    elevation: 1,
  },
  actionItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 14,
    paddingHorizontal: 16,
    backgroundColor: '#ffffff',
    borderBottomWidth: 1,
    borderBottomColor: '#f5f5f5',
  },
  actionLeft: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#ffffff',
  },
  actionLabel: {
    fontSize: 15,
    color: '#1a1a1a',
    marginLeft: 12,
    backgroundColor: '#ffffff',
  },
  logoutText: {
    color: '#e74c3c',
  },
modalOverlay: {
  flex: 1,
  justifyContent: 'center',
  alignItems: 'center',
  backgroundColor: 'rgba(0, 0, 0, 0.5)',
},
modalContent: {
  width: '80%',
  backgroundColor: '#ffffff',
  borderRadius: 12,
  padding: 20,
  shadowColor: '#000',
  shadowOffset: { width: 0, height: 2 },
  shadowOpacity: 0.25,
  shadowRadius: 4,
  elevation: 5,
},
modalTitle: {
  fontSize: 20,
  fontWeight: 'bold',
  color: '#1a1a1a',
  marginBottom: 12,
  textAlign: 'center',
},
modalText: {
  fontSize: 16,
  color: '#666',
  textAlign: 'center',
  marginBottom: 24,
},
modalButtons: {
  flexDirection: 'row',
  justifyContent: 'space-between',
  backgroundColor: '#ffffff',
},
modalButton: {
  flex: 1,
  paddingVertical: 12,
  borderRadius: 8,
  alignItems: 'center',
},
cancelButton: {
  backgroundColor: '#f0f0f0',
  marginRight: 10,
},
logoutButton: {
  backgroundColor: '#e74c3c',
  marginLeft: 10,
},
cancelButtonText: {
  color: '#666',
  fontWeight: '600',
},
logoutButtonText: {
  color: '#ffffff',
  fontWeight: '600',
},
  infoIconContainer: {
  flexDirection: 'row',
  alignItems: 'center',
  flex: 1,
  backgroundColor:'#fff'
},
infoValueContainer: {
  flexDirection: 'row',
  alignItems: 'center',
  flex: 2,
  justifyContent: 'flex-end',
  backgroundColor:'#fff'
}
});
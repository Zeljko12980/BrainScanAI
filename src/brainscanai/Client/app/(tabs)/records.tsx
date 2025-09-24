import { StyleSheet, ScrollView, Image, ActivityIndicator, TouchableOpacity, Modal } from 'react-native';
import { Text, View } from '@/components/Themed';
import { Ionicons } from '@expo/vector-icons';
import React, { useEffect, useState } from 'react';
import { useAuthStore } from '../store/authStore';
import { usePatientStore } from '../store/patientStore';



export default function RecordsScreen() {
  const { patient, loading, error, getPatientById } = usePatientStore();
  const {user} = useAuthStore();
 const [refreshing, setRefreshing] = useState(false);
 const [selectedImage, setSelectedImage] = useState<string | null>(null);
const [modalVisible, setModalVisible] = useState(false);

  const loadPatientData = async () => {
    try {
      await getPatientById(user?.id!);
    } finally {
      setRefreshing(false);
    }
  };

  useEffect(() => {
    loadPatientData();
  }, [user?.id]);

  const onRefresh = () => {
    setRefreshing(true);
    loadPatientData();
  };

  if (loading && !refreshing) return (
    <View style={styles.loadingContainer}>
      <ActivityIndicator size="large" color="#4a6cf7" />
    </View>
  );
  
  if (error) return (
    <View style={styles.errorContainer}>
      <Ionicons name="warning" size={48} color="#ff3b30" />
      <Text style={styles.errorText}>Error: {error}</Text>
    </View>
  );
  
  if (!patient) return (
    <View style={styles.emptyContainer}>
      <Ionicons name="person-outline" size={48} color="#cccccc" />
      <Text style={styles.emptyText}>No patient found</Text>
    </View>
  );

  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
        {/* Header */}
        <View style={styles.header}>
          <Text style={styles.title}>Medical Records</Text>
          <Text style={styles.subtitle}>Patient Treatment Overview</Text>
        </View>

        {/* Patient Information */}
        <View style={styles.card}>
          <View style={styles.cardHeader}>
            <Ionicons name="person-circle" size={24} color="#4a6cf7" />
            <Text style={styles.cardTitle}>Patient Information</Text>
          </View>
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Name:</Text>
            <Text style={styles.infoValue}>{patient.firstName} {patient.lastName}</Text>
          </View>
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Date of Birth:</Text>
            <Text style={styles.infoValue}>{patient.dateOfBirth}</Text>
          </View>
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Gender:</Text>
            <Text style={styles.infoValue}>{patient.gender}</Text>
          </View>
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Blood Type:</Text>
            <Text style={styles.infoValue}>{patient.bloodType}</Text>
          </View>
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Phone:</Text>
            <Text style={styles.infoValue}>{patient.phone}</Text>
          </View>
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Email:</Text>
            <Text style={styles.infoValue}>{patient.email}</Text>
          </View>
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Address:</Text>
            <Text style={styles.infoValue}>{patient.address}</Text>
          </View>
        </View>

        {/* Medical History */}
        <View style={styles.card}>
          <View style={styles.cardHeader}>
            <Ionicons name="document-text" size={24} color="#4a6cf7" />
            <Text style={styles.cardTitle}>Medical History</Text>
          </View>
          {patient.medicalHistory.events.length > 0 ? (
            patient.medicalHistory.events.map(event => (
              <View key={event.id} style={styles.historyItem}>
  {/* Type on its own line */}
  <Text style={styles.historyItemTitle}>{event.type}</Text>
  
  {/* Date on new line below */}
  <Text style={styles.historyItemDate}>{event.date}</Text>
  
  {/* Description on new line below with top margin */}
  <Text style={styles.historyItemText}>{event.description}</Text>
</View>
            ))
          ) : (
            <View style={styles.emptySection}>
              <Ionicons name="document-outline" size={32} color="#cccccc" />
              <Text style={styles.emptySectionText}>No medical history recorded</Text>
            </View>
          )}
        </View>

        {/* Medications */}
        <View style={styles.card}>
          <View style={styles.cardHeader}>
            <Ionicons name="medkit" size={24} color="#4a6cf7" />
            <Text style={styles.cardTitle}>Current Medications</Text>
          </View>
          {patient.medications.length > 0 ? (
            patient.medications.map((med, index) => (
              <View key={index} style={styles.medicationItem}>
               <Ionicons name="medical" size={20} color="#4a6cf7" />
                <Text style={styles.medicationText}>{med}</Text>
              </View>
            ))
          ) : (
            <View style={styles.emptySection}>
              <Ionicons name="medkit-outline" size={32} color="#cccccc" />
              <Text style={styles.emptySectionText}>No current medications</Text>
            </View>
          )}
        </View>

        {/* Scan Images */}
      
            {patient.scanImages.length > 0 && (
  <View style={styles.card}>
    <View style={styles.cardHeader}>
      <Ionicons name="scan" size={24} color="#4a6cf7" />
      <Text style={styles.cardTitle}>Scan Images</Text>
    </View>
    <ScrollView horizontal showsHorizontalScrollIndicator={false}>
      {patient.scanImages.map((img, index) => (
       <TouchableOpacity 
    key={index} 
    onPress={() => {
      setSelectedImage(img);
      setModalVisible(true);
    }}
  >
    <Image
      source={{ uri: `data:image/jpeg;base64,${img}` }}
      style={styles.scanImage}
    />
  </TouchableOpacity>
      ))}
    </ScrollView>
  </View>
)}

<Modal
  animationType="fade"
  transparent={true}
  visible={modalVisible}
  onRequestClose={() => setModalVisible(false)}
>
  <View style={styles.modalContainer}>
    <TouchableOpacity 
      style={styles.modalCloseButton}
      onPress={() => setModalVisible(false)}
    >
      <Ionicons name="close" size={30} color="white" />
    </TouchableOpacity>
    <Image
      source={{ uri: `data:image/jpeg;base64,${selectedImage}` }}
      style={styles.modalImage}
      resizeMode="contain"
    />
  </View>
</Modal>
         

        {/* Emergency Contact */}
        <View style={styles.card}>
          <View style={styles.cardHeader}>
            <Ionicons name="alert-circle" size={24} color="#4a6cf7" />
            <Text style={styles.cardTitle}>Emergency Contact</Text>
          </View>
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Name:</Text>
            <Text style={styles.infoValue}>{patient.emergencyContactName}</Text>
          </View>
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Phone:</Text>
            <Text style={styles.infoValue}>{patient.emergencyContactPhone}</Text>
          </View>
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Relation:</Text>
            <Text style={styles.infoValue}>{patient.emergencyContactRelation}</Text>
          </View>
        </View>
      </ScrollView>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#ffffff',
    marginTop:20
  },
  scrollContent: {
    padding: 20,
    paddingBottom: 40,
  },
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#ffffff',
  },
  errorContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#ffffff',
    padding: 20,
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#ffffff',
    padding: 20,
  },
  errorText: {
    fontSize: 16,
    color: '#ff3b30',
    marginTop: 16,
    textAlign: 'center',
    fontWeight: '600',
  },
  emptyText: {
    fontSize: 16,
    color: '#666666',
    marginTop: 16,
    textAlign: 'center',
  },
  header: {
    marginBottom: 24,
    backgroundColor: '#ffffff',
  },
  title: {
    fontSize: 26,
    fontWeight: '800',
    color: '#000000',
    marginBottom: 8,
  },
  subtitle: {
    fontSize: 16,
    color: '#666666',
    fontWeight: '500',
  },
  card: {
    backgroundColor: '#ffffff',
    borderRadius: 16,
    padding: 20,
    borderWidth: 1,
    borderColor: '#f0f0f0',
    marginBottom: 20,
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
    backgroundColor:'#fff'
  },
  cardTitle: {
    fontSize: 18,
    fontWeight: '700',
    color: '#000000',
    marginLeft: 8,
  },
  infoRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    backgroundColor:'#fff',
    marginBottom: 12,
    paddingHorizontal: 4,
  },
  infoLabel: {
    fontSize: 15,
    color: '#666666',
    fontWeight: '600',
     backgroundColor:'#fff',
  },
  infoValue: {
    fontSize: 15,
    color: '#333333',
    fontWeight: '500',
     backgroundColor:'#fff',
  },
  historyItem: {
    marginBottom: 16,
    paddingBottom: 16,
    borderBottomWidth: 1,
    borderBottomColor: '#f5f5f5',
     backgroundColor:'#fff',
  },
  historyItemHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
     backgroundColor:'#fff',
  },
  historyItemTitle: {
    fontSize: 15,
    fontWeight: '600',
    color: '#000000',
  },
  historyItemDate: {
    fontSize: 14,
    color: '#666666',
  },
  historyItemText: {
    fontSize: 14,
    color: '#333333',
    lineHeight: 20,
  },
  medicationItem: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 12,
    padding: 12,
    backgroundColor: 'rgba(74, 108, 247, 0.1)',
    borderRadius: 8,
  },
  medicationText: {
    fontSize: 15,
    color: '#333333',
    marginLeft: 10,
    fontWeight: '500',
  },
  scanImage: {
    width: 200,
    height: 200,
    borderRadius: 8,
    marginRight: 10,
  },
  emptySection: {
    alignItems: 'center',
    justifyContent: 'center',
    padding: 16,
  },
  emptySectionText: {
    fontSize: 14,
    color: '#666666',
    marginTop: 8,
    textAlign: 'center',
  },
  modalContainer: {
  flex: 1,
  backgroundColor: 'rgba(0,0,0,0.9)',
  justifyContent: 'center',
  alignItems: 'center',
},
modalImage: {
  width: '100%',
  height: '80%',
},
modalCloseButton: {
  position: 'absolute',
  top: 40,
  right: 20,
  zIndex: 1,
},

});
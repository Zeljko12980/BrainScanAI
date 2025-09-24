import React, { useEffect, useState } from 'react';
import { View, Text, FlatList, ActivityIndicator, StyleSheet, Alert, Button, Modal, TouchableOpacity, useColorScheme } from 'react-native';

import * as ImagePicker from 'expo-image-picker';
import { ImagePickerAsset } from 'expo-image-picker';
import { AntDesign, MaterialIcons } from '@expo/vector-icons';
import { Colors } from 'react-native/Libraries/NewAppScreen';
import { Image } from 'react-native';
import { usePatientStore, PatientDto } from '../store/patientStore';

type AnalysisResult = {
  tumorType?: string;
  confidence?: number;
  analysisDate?: string;
};

export default function PatientsScreen() {
  const { patients, loading, error, getAllPatients } = usePatientStore();
  const [selectedPatient, setSelectedPatient] = useState<PatientDto|null>(null);
  const [uploadModalVisible, setUploadModalVisible] = useState(false);
  const [resultModalVisible, setResultModalVisible] = useState(false);
  const [image, setImage] = useState<ImagePickerAsset | null>(null);
  const [uploading, setUploading] = useState(false);
  const colorScheme = useColorScheme();
  const [analysisResult, setAnalysisResult] = useState<AnalysisResult | null>(null);
  const [refreshing, setRefreshing] = useState(false);
 const loadPatients = async () => {
    try {
      await getAllPatients();
    } finally {
      setRefreshing(false);
    }
  };

  useEffect(() => {
    loadPatients();
  }, []);

  const onRefresh = () => {
    setRefreshing(true);
    loadPatients();
  };

  const pickImage = async () => {
    try {
      const { status } = await ImagePicker.requestMediaLibraryPermissionsAsync();
      if (status !== 'granted') {
        Alert.alert('Permission required', 'We need camera roll permissions to upload images');
        return;
      }

      const result = await ImagePicker.launchImageLibraryAsync({
        mediaTypes: ImagePicker.MediaTypeOptions.Images,
        allowsEditing: false,
        aspect: undefined,
        quality: 1,
        base64: true,
      });

      if (!result.canceled && result.assets && result.assets.length > 0) {
        setImage(result.assets[0]);
      }
    } catch (error) {
      console.error('Image picker error:', error);
      Alert.alert('Error', 'Failed to pick image');
    }
  };

  const uploadImage = async () => {
    if (!image || !selectedPatient || !image.base64) {
      Alert.alert('Error', 'Please select an image first');
      return;
    }

    setUploading(true);
    try {
      const response = await fetch('http://192.168.0.13:6004/doctor-service/api/tumor-analysis', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          imageBytes: image.base64,
          patientId: selectedPatient.id
        }),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const data = await response.json();
      setAnalysisResult(data);
      setResultModalVisible(true);
    } catch (error) {
      console.error('Upload error:', error);
      Alert.alert('Error', 'Failed to upload image. Please try again.');
    } finally {
      setUploading(false);
      setUploadModalVisible(false);
      setImage(null);
    }
  };

  const openPatientMenu = (patient: React.SetStateAction<PatientDto | null>) => {
    setSelectedPatient(patient);
    setUploadModalVisible(true);
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" color="#007AFF" />
      </View>
    );
  }

  if (error) {
    return (
      <View style={styles.center}>
        <Text style={styles.errorText}>{error}</Text>
      </View>
    );
  }

  const renderCard = ({ item }) => (
    <TouchableOpacity 
      style={styles.card}
      onPress={() => openPatientMenu(item)}
    >
      <Text style={styles.name}>
        {item.firstName} {item.lastName}
      </Text>
      <View style={styles.divider} />
      <View style={styles.detailRow}>
        <Text style={styles.label}>DOB:</Text>
        <Text style={styles.value}>
          {new Date(item.dateOfBirth).toLocaleDateString()}
        </Text>
      </View>
      <View style={styles.detailRow}>
        <Text style={styles.label}>Gender:</Text>
        <Text style={styles.value}>{item.gender}</Text>
      </View>
      <View style={styles.detailRow}>
        <Text style={styles.label}>Phone:</Text>
        <Text style={styles.value}>{item.phone}</Text>
      </View>
      <View style={styles.detailRow}>
        <Text style={styles.label}>Email:</Text>
        <Text style={styles.value}>{item.email}</Text>
      </View>
      <View style={styles.detailRow}>
        <Text style={styles.label}>Blood Type:</Text>
        <Text style={[
          styles.value,
          { 
            fontStyle: item.bloodType ? 'normal' : 'italic',
            color: item.bloodType ? '#000' : '#999'
          }
        ]}>
          {item.bloodType || 'Not specified'}
        </Text>
      </View>
    </TouchableOpacity>
  );

  return (
    <View style={styles.container}>
      <Text style={styles.header}>
        Patient List
      </Text>
      
      {/* Upload Image Modal */}
      <Modal
        animationType="slide"
        transparent={true}
        visible={uploadModalVisible}
        onRequestClose={() => {
          setUploadModalVisible(false);
        }}
      >
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <TouchableOpacity 
              style={styles.closeButton}
              onPress={() => setUploadModalVisible(false)}
            >
              <AntDesign name="close" size={24} color="#000" />
            </TouchableOpacity>
            
            <Text style={styles.modalTitle}>
              Upload Tumor Image for {selectedPatient?.firstName} {selectedPatient?.lastName}
            </Text>
            
            {image ? (
              <Image 
                source={{ uri: image.uri }} 
                style={styles.imagePreview} 
                resizeMode="contain"
              />
            ) : (
              <View style={styles.placeholder}>
                <Text style={styles.placeholderText}>
                  No image selected
                </Text>
              </View>
            )}
            
            <View style={styles.buttonContainer}>
              <Button 
                title="Select Image" 
                onPress={pickImage}
                color="#007AFF"
                disabled={uploading}
              />
              
              <TouchableOpacity
                style={[styles.submitButton, { opacity: (!image || uploading) ? 0.5 : 1 }]}
                onPress={uploadImage}
                disabled={!image || uploading}
              >
                <MaterialIcons name="check" size={24} color="white" />
                <Text style={styles.submitButtonText}>Submit</Text>
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </Modal>

      {/* Results Modal */}
      <Modal
        animationType="slide"
        transparent={true}
        visible={resultModalVisible}
        onRequestClose={() => {
          setResultModalVisible(false);
        }}
      >
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <TouchableOpacity 
              style={styles.closeButton}
              onPress={() => setResultModalVisible(false)}
            >
              <AntDesign name="close" size={24} color="#000" />
            </TouchableOpacity>
            
            <Text style={styles.modalTitle}>
              Analysis Results for {selectedPatient?.firstName} {selectedPatient?.lastName}
            </Text>
            
            <View style={styles.resultContainer}>
              <View style={styles.resultRow}>
                <Text style={styles.resultLabel}>Tumor Type:</Text>
                <Text style={styles.resultValue}>
                  {analysisResult?.tumorType?.charAt(0).toUpperCase()! + analysisResult?.tumorType?.slice(1) || 'N/A'}
                </Text>
              </View>
              
              <View style={styles.resultRow}>
                <Text style={styles.resultLabel}>Confidence:</Text>
                <Text style={styles.resultValue}>
                  {analysisResult?.confidence ? `${(analysisResult.confidence * 100).toFixed(2)}%` : 'N/A'}
                </Text>
              </View>
              
              <View style={styles.resultRow}>
                <Text style={styles.resultLabel}>Analysis Date:</Text>
                <Text style={styles.resultValue}>
                  {analysisResult?.analysisDate ? new Date(analysisResult.analysisDate).toLocaleString() : 'N/A'}
                </Text>
              </View>
            </View>
          </View>
        </View>
      </Modal>

      <FlatList
        data={patients}
        keyExtractor={(item) => item.id}
        renderItem={renderCard}
        contentContainerStyle={styles.listContent}
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Text style={styles.emptyText}>
              No patients found
            </Text>
          </View>
        }
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    paddingTop: 24,
    backgroundColor: 'white',
  },
  center: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'white',
  },
  errorText: {
    color: 'red',
    fontSize: 16,
  },
  header: {
    fontSize: 28,
    fontWeight: '700',
    paddingHorizontal: 24,
    marginBottom: 8,
    color: 'black',
  },
  listContent: {
    paddingHorizontal: 20,
    paddingBottom: 24,
  },
  card: {
    padding: 20,
    borderRadius: 12,
    marginBottom: 16,
    backgroundColor: 'white',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 6,
    elevation: 3,
  },
  name: {
    fontSize: 20,
    fontWeight: '600',
    marginBottom: 12,
    color: 'black',
  },
  divider: {
    height: 1,
    backgroundColor: '#f0f0f0',
    marginVertical: 8,
  },
  detailRow: {
    flexDirection: 'row',
    marginBottom: 8,
  },
  label: {
    width: 100,
    fontSize: 14,
    fontWeight: '500',
    color: '#666',
  },
  value: {
    flex: 1,
    fontSize: 14,
    color: 'black',
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 40,
  },
  emptyText: {
    fontSize: 16,
    textAlign: 'center',
    color: '#666',
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0,0,0,0.5)',
  },
  modalContent: {
    width: '90%',
    padding: 20,
    borderRadius: 10,
    backgroundColor: 'white',
    elevation: 5,
  },
  closeButton: {
    alignSelf: 'flex-end',
    marginBottom: 10,
  },
  modalTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    marginBottom: 20,
    textAlign: 'center',
    color: 'black',
  },
  imagePreview: {
    width: '100%',
    height: 200,
    marginBottom: 20,
    borderRadius: 5,
  },
  placeholder: {
    width: '100%',
    height: 200,
    justifyContent: 'center',
    alignItems: 'center',
    borderWidth: 1,
    borderColor: '#ccc',
    borderRadius: 5,
    marginBottom: 20,
  },
  placeholderText: {
    color: '#999',
  },
  buttonContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 10,
  },
  submitButton: {
    flexDirection: 'row',
    backgroundColor: '#007AFF',
    paddingVertical: 10,
    paddingHorizontal: 20,
    borderRadius: 5,
    alignItems: 'center',
    justifyContent: 'center',
  },
  submitButtonText: {
    color: 'white',
    marginLeft: 5,
    fontWeight: 'bold',
  },
  resultContainer: {
    marginTop: 10,
    padding: 15,
    borderRadius: 8,
    backgroundColor: '#f9f9f9',
  },
  resultRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
  },
  resultLabel: {
    fontSize: 14,
    fontWeight: '600',
    color: '#666',
  },
  resultValue: {
    fontSize: 14,
    fontWeight: '500',
    color: 'black',
  },
});
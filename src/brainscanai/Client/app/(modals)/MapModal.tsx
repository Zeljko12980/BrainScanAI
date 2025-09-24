import React from 'react';
import { Modal, View, StyleSheet, TouchableOpacity, Text, Linking, Alert } from 'react-native';
import { Ionicons } from '@expo/vector-icons';

interface MapModalProps {
  visible: boolean;
  onClose: () => void;
  location: string;
  latitude: number;
  longitude: number;
}

const FOCA_HOSPITAL = {
  name: "Opšta bolnica Foča",
  address: "Save Kovačevića bb, Foča 73300, Bosna i Hercegovina",
  latitude: 43.5083,
  longitude: 18.7786
};

const MapModal: React.FC<MapModalProps> = ({ visible, onClose }) => {
  const openDirections = () => {
    const url = `https://www.google.com/maps/dir/?api=1&destination=${FOCA_HOSPITAL.latitude},${FOCA_HOSPITAL.longitude}`;
    
    Linking.canOpenURL(url).then(supported => {
      if (supported) {
        Linking.openURL(url);
      } else {
        Alert.alert(
          "Error",
          "Could not open the map. Please try again later.",
          [{ text: "OK" }]
        );
      }
    });
  };

  return (
    <Modal
      visible={visible}
      transparent={false}
      animationType="slide"
      onRequestClose={onClose}
    >
      <View style={styles.container}>
        <View style={styles.header}>
          <Text style={styles.locationText}>{FOCA_HOSPITAL.name}</Text>
          <TouchableOpacity onPress={onClose} style={styles.closeButton}>
            <Ionicons name="close" size={24} color="#000" />
          </TouchableOpacity>
        </View>
        
        <View style={styles.content}>
          <Ionicons name="location" size={48} color="#4a6cf7" style={styles.locationIcon} />
          <Text style={styles.addressText}>{FOCA_HOSPITAL.address}</Text>
          
          <TouchableOpacity 
            style={styles.directionsButton}
            onPress={openDirections}
          >
            <Ionicons name="navigate" size={20} color="#fff" />
            <Text style={styles.directionsButtonText}>Get Directions</Text>
          </TouchableOpacity>
        </View>
      </View>
    </Modal>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: 15,
    backgroundColor: '#fff',
    borderBottomWidth: 1,
    borderBottomColor: '#eee',
  },
  locationText: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#000',
  },
  closeButton: {
    padding: 5,
  },
  content: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
  },
  locationIcon: {
    marginBottom: 20,
  },
  addressText: {
    fontSize: 16,
    color: '#666',
    marginBottom: 30,
    textAlign: 'center',
    lineHeight: 24,
  },
  directionsButton: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#4a6cf7',
    padding: 15,
    borderRadius: 8,
    width: '80%',
  },
  directionsButtonText: {
    color: '#fff',
    fontWeight: 'bold',
    marginLeft: 10,
    fontSize: 16,
  },
});

export default MapModal;
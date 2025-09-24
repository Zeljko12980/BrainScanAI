import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  TextInput,
  StyleSheet,
  Alert,
  TouchableOpacity,
  KeyboardAvoidingView,
  Platform,
  SafeAreaView,
  ActivityIndicator,
  Animated,
  Easing,
  Dimensions
} from 'react-native';
import { LinearGradient } from 'expo-linear-gradient';
import { Ionicons } from '@expo/vector-icons';

import { router } from 'expo-router';
import { useAuthStore } from '../store/authStore';

const { width } = Dimensions.get('window');

export default function Verify2FAScreen() {
  const [code, setCode] = useState('');
  const [shakeAnimation] = useState(new Animated.Value(0));
  const [buttonScale] = useState(new Animated.Value(1));
  

  const { verify2FA, isLoading, error, clearError } = useAuthStore();

  const handleVerify = async () => {
    if (!code || code.length !== 6) {
      triggerShake();
      Alert.alert('Verification Required', 'Please enter the 6-digit code sent to your email');
      return;
    }

    try {
      await verify2FA(code);
    
    } catch (err) {
      triggerShake();
    
    }
  };


  useEffect(() => {
    if (error) {
      Alert.alert('Verification Failed', error);
      clearError();
    }
  }, [error]);

  const triggerShake = () => {
    Animated.sequence([
      Animated.timing(shakeAnimation, {
        toValue: 10,
        duration: 50,
        easing: Easing.linear,
        useNativeDriver: true
      }),
      Animated.timing(shakeAnimation, {
        toValue: -10,
        duration: 50,
        easing: Easing.linear,
        useNativeDriver: true
      }),
      Animated.timing(shakeAnimation, {
        toValue: 10,
        duration: 50,
        easing: Easing.linear,
        useNativeDriver: true
      }),
      Animated.timing(shakeAnimation, {
        toValue: 0,
        duration: 50,
        easing: Easing.linear,
        useNativeDriver: true
      })
    ]).start();
  };

  const handlePressIn = () => {
    Animated.spring(buttonScale, {
      toValue: 0.95,
      useNativeDriver: true,
    }).start();
  };

  const handlePressOut = () => {
    Animated.spring(buttonScale, {
      toValue: 1,
      useNativeDriver: true,
    }).start();
  };

  const handleResendCode = async () => {
    try {
      Alert.alert('Code Resent', 'A new verification code has been sent to your email');
    } catch (err) {
      Alert.alert('Error', 'Failed to resend verification code');
    }
  };

  return (
    <LinearGradient 
      colors={['#f8fdff', '#d6f0ff']} 
      style={styles.gradient}
      start={{ x: 0, y: 0 }}
      end={{ x: 1, y: 1 }}
    >
      <SafeAreaView style={styles.safeArea}>
        <KeyboardAvoidingView
          style={styles.container}
          behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
          keyboardVerticalOffset={Platform.OS === 'ios' ? 60 : 0}
        >
          <Animated.View style={[styles.content, {
            transform: [{ translateX: shakeAnimation }]
          }]}>
            <View style={styles.header}>
              <View style={styles.iconContainer}>
                <Ionicons name="shield-checkmark" size={36} color="#4a6cf7" />
              </View>
              <Text style={styles.title}>Secure Verification</Text>
              <Text style={styles.subtitle}>
                For your security, we've sent a 6-digit code to your registered email address
              </Text>
            </View>

            <View style={styles.inputContainer}>
              <Text style={styles.inputLabel}>VERIFICATION CODE</Text>
              <TextInput
                style={styles.input}
                placeholder="• • • • • •"
                placeholderTextColor="#ccc"
                keyboardType="number-pad"
                maxLength={6}
                value={code}
                onChangeText={setCode}
                autoFocus={true}
                selectionColor="#4a6cf7"
              />
            </View>

            <Animated.View style={{ transform: [{ scale: buttonScale }] }}>
              <TouchableOpacity 
                style={[styles.button, isLoading && styles.buttonDisabled]} 
                onPress={handleVerify}
                onPressIn={handlePressIn}
                onPressOut={handlePressOut}
                disabled={isLoading}
                activeOpacity={0.8}
              >
                <LinearGradient
                  colors={['#5a7cfe', '#4a6cf7']}
                  style={styles.buttonGradient}
                  start={{ x: 0, y: 0 }}
                  end={{ x: 1, y: 0 }}
                >
                  {isLoading ? (
                    <ActivityIndicator color="#fff" />
                  ) : (
                    <>
                      <Ionicons name="checkmark-circle" size={20} color="white" style={styles.buttonIcon} />
                      <Text style={styles.buttonText}>Verify Identity</Text>
                    </>
                  )}
                </LinearGradient>
              </TouchableOpacity>
            </Animated.View>

            <View style={styles.footer}>
              <Text style={styles.footerText}>Didn't receive the code?</Text>
              <TouchableOpacity 
                style={styles.resendButton}
                onPress={handleResendCode}
                disabled={isLoading}
              >
                <Text style={styles.resendLink}>Resend Code</Text>
                <Ionicons name="refresh" size={16} color="#4a6cf7" style={styles.resendIcon} />
              </TouchableOpacity>
            </View>
          </Animated.View>
        </KeyboardAvoidingView>
      </SafeAreaView>
    </LinearGradient>
  );
}

const styles = StyleSheet.create({
  gradient: {
    flex: 1,
  },
  safeArea: {
    flex: 1,
  },
  container: {
    flex: 1,
    justifyContent: 'center',
    padding: 24,
  },
  content: {
    width: '100%',
  },
  header: {
    alignItems: 'center',
    marginBottom: 40,
  },
  iconContainer: {
    backgroundColor: 'rgba(74, 108, 247, 0.1)',
    width: 80,
    height: 80,
    borderRadius: 40,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 20,
  },
  title: {
    fontSize: 26,
    fontWeight: '800',
    color: '#1a1a1a',
    marginBottom: 12,
    textAlign: 'center',
  },
  subtitle: {
    fontSize: 15,
    color: '#666',
    textAlign: 'center',
    lineHeight: 22,
    paddingHorizontal: 20,
    fontWeight: '500',
  },
  inputContainer: {
    marginBottom: 30,
  },
  inputLabel: {
    fontSize: 12,
    fontWeight: '700',
    color: '#666',
    marginBottom: 10,
    letterSpacing: 1,
    textAlign: 'center',
  },
  input: {
    height: 60,
    backgroundColor: '#fff',
    borderColor: '#e1e5eb',
    borderWidth: 1,
    borderRadius: 14,
    paddingHorizontal: 16,
    fontSize: 24,
    fontWeight: '600',
    color: '#1a1a1a',
    letterSpacing: 8,
    textAlign: 'center',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.05,
    shadowRadius: 4,
    elevation: 2,
  },
  button: {
    borderRadius: 14,
    marginTop: 20,
    overflow: 'hidden',
    shadowColor: '#4a6cf7',
    shadowOffset: { width: 0, height: 6 },
    shadowOpacity: 0.25,
    shadowRadius: 12,
    elevation: 8,
  },
  buttonGradient: {
    paddingVertical: 18,
    borderRadius: 14,
    alignItems: 'center',
    justifyContent: 'center',
    flexDirection: 'row',
  },
  buttonDisabled: {
    opacity: 0.8,
  },
  buttonText: {
    color: '#fff',
    fontSize: 18,
    fontWeight: '600',
    marginLeft: 10,
  },
  buttonIcon: {
    marginRight: 8,
  },
  footer: {
    marginTop: 32,
    alignItems: 'center',
  },
  footerText: {
    color: '#666',
    fontSize: 14,
  },
  resendButton: {
    flexDirection: 'row',
    alignItems: 'center',
    marginTop: 8,
  },
  resendLink: {
    color: '#4a6cf7',
    fontSize: 14,
    fontWeight: '600',
  },
  resendIcon: {
    marginLeft: 6,
  },
});
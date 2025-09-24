import React, { useEffect } from 'react';
import FontAwesome from '@expo/vector-icons/FontAwesome';
import { Link, Tabs, useRouter } from 'expo-router';
import { ActivityIndicator, Pressable, View } from 'react-native';

import Colors from '@/constants/Colors';
import { useColorScheme } from '@/components/useColorScheme';
import { useClientOnlyValue } from '@/components/useClientOnlyValue';
import { useAuthStore } from '../store/authStore';
import { Ionicons } from '@expo/vector-icons';

// You can explore the built-in icon families and icons on the web at https://icons.expo.fyi/
function TabBarIcon(props: {
  name: React.ComponentProps<typeof FontAwesome>['name'];
  color: string;
}) {
  return <FontAwesome size={28} style={{ marginBottom: -3 }} {...props} />;
}

export default function TabLayout() {
  const colorScheme = useColorScheme();
  const { user, isLoading } = useAuthStore();
  const router = useRouter();

  // Handle redirection in useEffect to prevent render-loop
  useEffect(() => {
    if (!isLoading && !user) {
      router.replace("/(screen)/WelcomeScreen");
    }
  }, [user, isLoading]);

  if (isLoading || !user) {
    return (
      <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
        <ActivityIndicator size="large" color={Colors[colorScheme ?? 'light'].tint} />
      </View>
    );
  }

    const isPatient = user.role === 'Patient';
  return (
 <Tabs
      screenOptions={{
        tabBarActiveTintColor: Colors['light'].tint,
        headerShown: false,
        tabBarStyle: {
          backgroundColor: Colors['light'].background,
          borderTopWidth: 0,
          elevation: 0,
        },
        tabBarLabelStyle: {
          fontSize: 12,
          fontWeight: '600',
        },
      }}
    >
          <Tabs.Screen name="index" options={{
        title: 'Home',
        tabBarIcon: ({ color }) => <Ionicons name="home-outline" size={22} color={color} />
      }} />
   <Tabs.Screen name="appointments" options={{
        title: 'Appointments',
        tabBarIcon: ({ color }) => <Ionicons name="calendar-outline" size={22} color={color} />
      }} />


   <Tabs.Screen
  name="records"
  options={{
    href: isPatient ? undefined : null, // ako nije pacijent → ne prikazuj tab
    title: 'Records',
    tabBarIcon: ({ color }) => (
      <Ionicons name="document-text-outline" size={22} color={color} />
    ),
  }}
/>

<Tabs.Screen
  name="patients"
  options={{
    href: !isPatient ? undefined : null, // ako je pacijent → ne prikazuj tab
    title: 'Patients',
    tabBarIcon: ({ color }) => (
      <Ionicons name="people-outline" size={22} color={color} />
    ),
  }}
/>


        <Tabs.Screen name="messages" options={{
        title: 'Messages',
        tabBarIcon: ({ color }) => <Ionicons name="chatbubble-ellipses-outline" size={22} color={color} />
      }} />
      
      <Tabs.Screen name="settings" options={{
        title: 'Settings',
        tabBarIcon: ({ color }) => <Ionicons name="settings-outline" size={22} color={color} />
      }} />
    
    </Tabs>
  );
}

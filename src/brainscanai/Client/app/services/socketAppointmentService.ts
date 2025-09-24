// services/socketAppointmentService.ts
import * as SignalR from '@microsoft/signalr';
import { useAppointmentStore } from '../store/appoitmentStore';


let appointmentConnection: SignalR.HubConnection | null = null;

export const initAppointmentSocket = (userId: string) => {
  if (appointmentConnection) return appointmentConnection;

  appointmentConnection = new SignalR.HubConnectionBuilder()
    .withUrl(`http://192.168.0.13:6004/appointment-service/hubs/appointments`)
    .withAutomaticReconnect()
    .build();

  // 📅 Handleri za dolazne evente iz AppointmentHub-a
  appointmentConnection.on('ReceiveAppointment', (appointment) => {
    console.log('📅 Novi appointment:', appointment);
    useAppointmentStore.getState().addAppointment(appointment);
  });

  appointmentConnection.on('AppointmentRescheduled', (appointment) => {
    console.log('🔄 Appointment pomjeren:', appointment);
    const { appointments } = useAppointmentStore.getState();
    useAppointmentStore.setState({
      appointments: appointments.map(a => a.id === appointment.id ? appointment : a),
      upcomingAppointments: appointments.map(a => a.id === appointment.id ? appointment : a)
    });
  });

  appointmentConnection.on('AppointmentCancelled', (appointment) => {
    console.log('❌ Appointment otkazan:', appointment);
    const { appointments, upcomingAppointments } = useAppointmentStore.getState();
    useAppointmentStore.setState({
      appointments: appointments.map(a => a.id === appointment.id ? { ...a, status: 'Cancelled' } : a),
      upcomingAppointments: upcomingAppointments.filter(a => a.id !== appointment.id)
    });
  });

  appointmentConnection.on('AppointmentConfirmed', (appointment) => {
    console.log('✅ Appointment potvrđen:', appointment);
    const { appointments, upcomingAppointments, selectedAppointment } = useAppointmentStore.getState();
    useAppointmentStore.setState({
      appointments: appointments.map(a => a.id === appointment.id ? { ...a, status: 'Confirmed' } : a),
      upcomingAppointments: upcomingAppointments.map(a => a.id === appointment.id ? { ...a, status: 'Confirmed' } : a),
     // selectedAppointment: selectedAppointment?.id === appointment.id ? { ...selectedAppointment, status: 'Confirmed' } : selectedAppointment
    });
  });

  appointmentConnection
    .start()
    .then(async () => {
      console.log('✅ SignalR (Appointments) connected');
      await appointmentConnection!.invoke('RegisterUserAsync', userId);
    })
    .catch((err) => console.error('❌ SignalR (Appointments) error:', err));

  return appointmentConnection;
};

export const stopAppointmentSocket = async () => {
  if (appointmentConnection) {
    await appointmentConnection.stop();
    appointmentConnection = null;
  }
};

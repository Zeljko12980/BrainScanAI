namespace Appointment.Application.Common.Interface
{
    public interface IAppointmentService
    {
        Task<AppointmentDto> ScheduleAppointment(Guid patientId,Guid doctorId,DateTime appointmentTime,TimeSpan duration,string location, string doctorName,string specialty,string patientName,string notes = null);
        Task<bool> RescheduleAppointment(Guid appointmentId, DateTime newAppointmentTime);
        Task<bool> CancelAppointment(Guid appointmentId);
        Task<bool> ConfirmAppointment(Guid appointmentId);
        Task<AppointmentDto> GetAppointmentById(Guid appointmentId);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatient(Guid patientId);
        Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctor(Guid doctorId, DateTime? date = null);
        Task<IEnumerable<AppointmentDto>> GetUpcomingAppointments(int daysAhead = 7);
    }
}

namespace Appointment.Application.Common.Dtos
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpecialty { get; set; }
        public string PatientName { get; set; }
    }
}

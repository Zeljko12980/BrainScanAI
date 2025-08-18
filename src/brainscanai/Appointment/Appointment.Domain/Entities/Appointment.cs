namespace Appointment.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; private set; }
        public Guid PatientId { get; private set; }
        public Guid DoctorId { get; private set; }
        public DateTime AppointmentTime { get; private set; }
        public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(20);
        public string Location { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public string Notes { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string DoctorName { get; private set; }
        public string Specialty { get; private set; }
        public string PatientName { get; private set; }

        private Appointment() { }

        public Appointment(
            Guid patientId,
            Guid doctorId,
            DateTime appointmentTime,
            string location,
            string doctorName,
            string specialty,
            string patientName,
            string notes = null,
            IEnumerable<Appointment> existingAppointments = null)
        {
            if (appointmentTime < DateTime.UtcNow.AddMinutes(30))
            {
                throw new ArgumentException("Appointments must be scheduled at least 30 minutes in advance");
            }

            if (appointmentTime.Minute % 20 != 0)
            {
                throw new ArgumentException("Appointments must start at 20-minute intervals");
            }

            if (existingAppointments != null)
            {
                var dailyAppointments = existingAppointments
                    .Where(a => a.AppointmentTime.Date == appointmentTime.Date &&
                               a.DoctorId == doctorId &&
                               a.Status != AppointmentStatus.Cancelled)
                    .ToList();

                if (dailyAppointments.Count >= 5)
                {
                    throw new InvalidOperationException("Maximum of 5 appointments per day reached for this doctor");
                }

                if (dailyAppointments.Any(a =>
                    (appointmentTime >= a.AppointmentTime && appointmentTime < a.AppointmentTime.Add(a.Duration)) ||
                    (appointmentTime.Add(Duration) > a.AppointmentTime && appointmentTime.Add(Duration) <= a.AppointmentTime.Add(a.Duration)) ||
                    (appointmentTime <= a.AppointmentTime && appointmentTime.Add(Duration) >= a.AppointmentTime.Add(a.Duration))))
                {
                    throw new InvalidOperationException("Time slot already booked");
                }
            }

            Id = Guid.NewGuid();
            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentTime = appointmentTime;
            Location = location;
            Status = AppointmentStatus.Scheduled;
            Notes = notes;
            CreatedAt = DateTime.UtcNow;
            DoctorName = doctorName;
            Specialty = specialty;
            PatientName = patientName;
        }

        public void Reschedule(DateTime newAppointmentTime, IEnumerable<Appointment> existingAppointments = null)
        {
            if (newAppointmentTime < DateTime.UtcNow.AddMinutes(30))
            {
                throw new ArgumentException("Appointments must be scheduled at least 30 minutes in advance");
            }

            if (newAppointmentTime.Minute % 20 != 0)
            {
                throw new ArgumentException("Appointments must start at 20-minute intervals");
            }

            if (existingAppointments != null)
            {
                var dailyAppointments = existingAppointments
                    .Where(a => a.AppointmentTime.Date == newAppointmentTime.Date &&
                               a.DoctorId == DoctorId &&
                               a.Status != AppointmentStatus.Cancelled &&
                               a.Id != Id)
                    .ToList();

                if (dailyAppointments.Count >= 5)
                {
                    throw new InvalidOperationException("Maximum of 5 appointments per day reached for this doctor");
                }

                if (dailyAppointments.Any(a =>
                    (newAppointmentTime >= a.AppointmentTime && newAppointmentTime < a.AppointmentTime.Add(a.Duration)) ||
                    (newAppointmentTime.Add(Duration) > a.AppointmentTime && newAppointmentTime.Add(Duration) <= a.AppointmentTime.Add(a.Duration)) ||
                    (newAppointmentTime <= a.AppointmentTime && newAppointmentTime.Add(Duration) >= a.AppointmentTime.Add(a.Duration))))
                {
                    throw new InvalidOperationException("Time slot already booked");
                }
            }

            AppointmentTime = newAppointmentTime;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == AppointmentStatus.Completed || Status == AppointmentStatus.NoShow)
            {
                throw new InvalidOperationException("Cannot cancel a completed or no-show appointment");
            }

            Status = AppointmentStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Confirm()
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                throw new InvalidOperationException("Only scheduled appointments can be confirmed");
            }

            Status = AppointmentStatus.Confirmed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete()
        {
            if (Status != AppointmentStatus.Confirmed)
            {
                throw new InvalidOperationException("Only confirmed appointments can be completed");
            }

            Status = AppointmentStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsNoShow()
        {
            if (Status != AppointmentStatus.Confirmed)
            {
                throw new InvalidOperationException("Only confirmed appointments can be marked as no-show");
            }

            Status = AppointmentStatus.NoShow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateNotes(string notes)
        {
            Notes = notes;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateDoctorInfo(string doctorName, string specialty)
        {
            DoctorName = doctorName;
            Specialty = specialty;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

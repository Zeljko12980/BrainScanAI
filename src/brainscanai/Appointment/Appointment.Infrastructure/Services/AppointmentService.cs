using Appointment.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Infrastructure.Services
{
    public class AppointmentService
        (AppointmentDbContext context,IMapper mapper)
        : IAppointmentService
    {
        public async Task<AppointmentDto> ScheduleAppointment(
      Guid patientId,
      Guid doctorId,
      DateTime appointmentTime,
      TimeSpan duration,
      string location,
      string doctorName,
      string specialty,
      string patientName,
      string notes = null)
        {
            var existingAppointments = await context.Appointments
                .Where(a => a.DoctorId == doctorId &&
                           a.AppointmentTime.Date == appointmentTime.Date &&
                           a.Status != AppointmentStatus.Cancelled)
                .ToListAsync();

            var appointment = new Domain.Entities.Appointment(
                patientId,
                doctorId,
                appointmentTime,
                location,
                doctorName,
                specialty,
                patientName,
                notes,
                existingAppointments)
            {
                Duration = duration
            };

            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            return mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<bool> RescheduleAppointment(Guid appointmentId, DateTime newAppointmentTime)
        {
            var appointment = await context.Appointments.FindAsync(appointmentId);
            if (appointment == null) return false;

           
            var existingAppointments = await context.Appointments
                .Where(a => a.DoctorId == appointment.DoctorId &&
                           a.AppointmentTime.Date == newAppointmentTime.Date &&
                           a.Id != appointmentId &&
                           a.Status != AppointmentStatus.Cancelled)
                .ToListAsync();

            try
            {
                appointment.Reschedule(newAppointmentTime, existingAppointments);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CancelAppointment(Guid appointmentId)
        {
            var appointment = await context.Appointments.FindAsync(appointmentId);
            if (appointment == null) return false;

            try
            {
                appointment.Cancel();
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ConfirmAppointment(Guid appointmentId)
        {
            var appointment = await context.Appointments.FindAsync(appointmentId);
            if (appointment == null) return false;

            try
            {
                appointment.Confirm();
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<AppointmentDto> GetAppointmentById(Guid appointmentId)
        {
            var appointment = await context.Appointments
                .FirstOrDefaultAsync(a => a.Id == appointmentId);

            return mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatient(Guid patientId)
        {
            var appointments = await context.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();

            return mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctor(Guid doctorId, DateTime? date = null)
        {
            var query = context.Appointments
                .Where(a => a.DoctorId == doctorId);

            if (date.HasValue)
            {
                query = query.Where(a => a.AppointmentTime.Date == date.Value.Date);
            }

            var appointments = await query
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();

            return mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointments(int daysAhead = 7)
        {
            var fromDate = DateTime.UtcNow;
            var toDate = fromDate.AddDays(daysAhead);

            var appointments = await context.Appointments
                .Where(a => a.AppointmentTime >= fromDate && a.AppointmentTime <= toDate)
                .OrderBy(a => a.AppointmentTime)
                .ToListAsync();

            return mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }
    }
}

namespace Appointment.Application.Command
{
    public record ScheduleAppointmentCommand(
      Guid PatientId,
      Guid DoctorId,
      DateTime AppointmentTime,
      TimeSpan Duration,
      string Location,
      string DoctorName,
      string Specialty,
      string PatientName,
      string Notes = null):ICommand<AppointmentDto>;

    public class ScheduleAppointmentCommandHandler
        (IAppointmentService appointmentService)
        : ICommandHandler<ScheduleAppointmentCommand, AppointmentDto>
    {
        public async Task<AppointmentDto> Handle(ScheduleAppointmentCommand command, CancellationToken cancellationToken)
        {
            var result = await appointmentService.ScheduleAppointment(
                command.PatientId,
                command.DoctorId,
                command.AppointmentTime,
                command.Duration,
                command.Location,
                command.DoctorName,
                command.Specialty,
                command.PatientName,
                command.Notes
                );

            return result;
        }
    }
}

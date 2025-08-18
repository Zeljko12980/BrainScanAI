namespace Appointment.Application.Command
{
    public record CancelAppointmentCommand(Guid AppointmentId):ICommand<bool>;

    public class CancelAppointmentCommandHandler
        (IAppointmentService appointmentService)
        : ICommandHandler<CancelAppointmentCommand, bool>
    {
        public async Task<bool> Handle(CancelAppointmentCommand command, CancellationToken cancellationToken)
        {
            var result = await appointmentService.CancelAppointment(command.AppointmentId);

            return result;
        }
    }
}

namespace Appointment.Application.Command
{
    public record ConfirmAppointmentCommand(Guid AppointmentId):ICommand<bool>;

    public class ConfirmAppointmentCommandHandler
        (IAppointmentService appointmentService)
        : ICommandHandler<ConfirmAppointmentCommand, bool>
    {
        public async Task<bool> Handle(ConfirmAppointmentCommand command, CancellationToken cancellationToken)
        {
            var result= await appointmentService.ConfirmAppointment(command.AppointmentId);

            return result;
        }
    }
}

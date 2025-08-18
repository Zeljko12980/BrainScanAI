
namespace Appointment.Application.Command
{
    public record RescheduleAppointmentCommand(
    Guid AppointmentId,
    DateTime NewAppointmentTime) : ICommand<bool>;

    public class RescheduleAppointmentCommandHandler
        (IAppointmentService appointmentService)
        : ICommandHandler<RescheduleAppointmentCommand, bool>
    {
        public async Task<bool> Handle(RescheduleAppointmentCommand command, CancellationToken cancellationToken)
        {
            var result= await appointmentService.RescheduleAppointment(command.AppointmentId, command.NewAppointmentTime);

            return result;
        }
    }


}


namespace Appointment.Application.Query
{
    public record GetUpcomingAppointmentsQuery(int DaysAhead = 7):IQuery<IEnumerable<AppointmentDto>>;

    public class GetUpcomingAppointmentsQueryHandler
        (IAppointmentService appointmentService)
        : IQueryHandler<GetUpcomingAppointmentsQuery, IEnumerable<AppointmentDto>>
    {
        public async Task<IEnumerable<AppointmentDto>> Handle(GetUpcomingAppointmentsQuery query, CancellationToken cancellationToken)
        {
            var result = await appointmentService.GetUpcomingAppointments(query.DaysAhead);

            return result;
        }
    }
}

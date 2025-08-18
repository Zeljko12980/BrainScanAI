namespace Appointment.Application.Query
{
    public record GetAppointmentsByDoctorQuery(Guid DoctorId, DateTime? Date = null):IQuery<IEnumerable<AppointmentDto>>;

    public class GetAppointmentsByDoctorQueryHandler
        (IAppointmentService appointmentService)
        : IQueryHandler<GetAppointmentsByDoctorQuery, IEnumerable<AppointmentDto>>
    {
        public async Task<IEnumerable<AppointmentDto>> Handle(GetAppointmentsByDoctorQuery query, CancellationToken cancellationToken)
        {
            var result = await appointmentService.GetAppointmentsByDoctor(query.DoctorId, query.Date);

            return result;
        }
    }
}

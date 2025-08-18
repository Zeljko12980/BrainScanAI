namespace Appointment.Application.Query
{
    public record GetAppointmentsByPatientQuery(Guid PatientId):IQuery<IEnumerable<AppointmentDto>>;

    public class GetAppointmentsByPatientQueryHandler
        (IAppointmentService appointmentService)
        : IQueryHandler<GetAppointmentsByPatientQuery, IEnumerable<AppointmentDto>>
    {
        public async Task<IEnumerable<AppointmentDto>> Handle(GetAppointmentsByPatientQuery query, CancellationToken cancellationToken)
        {
            var result = await appointmentService.GetAppointmentsByPatient(query.PatientId);

            return result;
        }
    }
}

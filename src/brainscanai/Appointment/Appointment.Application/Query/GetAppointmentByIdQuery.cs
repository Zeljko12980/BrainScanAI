namespace Appointment.Application.Query
{
    public record GetAppointmentByIdQuery(Guid AppointmentId):IQuery<AppointmentDto>;

    public class GetAppointmentByIdQueryHandler
        (IAppointmentService appointmentService)
        : IQueryHandler<GetAppointmentByIdQuery, AppointmentDto>
    {
        public async Task<AppointmentDto> Handle(GetAppointmentByIdQuery query, CancellationToken cancellationToken)
        {
            var result =await appointmentService.GetAppointmentById(query.AppointmentId);

            return result;
        }
    }
}

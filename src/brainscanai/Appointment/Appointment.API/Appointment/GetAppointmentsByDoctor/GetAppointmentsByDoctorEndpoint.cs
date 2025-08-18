
namespace Appointment.API.Appointment.GetAppointmentsByDoctor
{
    public class GetAppointmentsByDoctorEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/appointments/doctor/{doctorId:guid}", async (
                Guid doctorId,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAppointmentsByDoctorQuery(doctorId);
                var result = await mediator.Send(query, cancellationToken);

                return Results.Ok(result);
            })
            .WithName("GetAppointmentsByDoctor")
            .WithTags("Appointments")
            .Produces<IEnumerable<AppointmentDto>>()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get doctor appointments",
                Description = "Retrieves all appointments for a specific doctor"
            });
        }
    }
}

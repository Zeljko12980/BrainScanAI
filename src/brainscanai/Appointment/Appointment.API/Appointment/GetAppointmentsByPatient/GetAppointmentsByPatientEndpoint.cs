namespace Appointment.API.Appointment.GetAppointmentsByPatient
{
    public class GetAppointmentsByPatientEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/appointments/patient/{patientId:guid}", async (
                Guid patientId,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAppointmentsByPatientQuery(patientId);
                var result = await mediator.Send(query, cancellationToken);

                return Results.Ok(result);
            })
            .WithName("GetAppointmentsByPatient")
            .WithTags("Appointments")
            .Produces<IEnumerable<AppointmentDto>>()
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get patient appointments",
                Description = "Retrieves all appointments for a specific patient"
            });
        }
    }
}

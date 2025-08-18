namespace Appointment.API.Appointment.ScheduleAppointment
{
    public class ScheduleAppointmentEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/appointments", async (
                ScheduleAppointmentCommand command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
               
                var result = await mediator.Send(command, cancellationToken);

                return Results.CreatedAtRoute(
                    "GetAppointmentById",
                    new { id = result.Id },
                    result);
            })
            .WithName("ScheduleAppointment")
            .WithTags("Appointments")
            .Produces<AppointmentDto>(201)
            .ProducesProblem(400)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Schedule a new appointment",
                Description = "Creates a new medical appointment"
            });
        }
    }
}

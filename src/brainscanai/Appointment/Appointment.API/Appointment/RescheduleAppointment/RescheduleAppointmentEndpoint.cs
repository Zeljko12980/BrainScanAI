namespace Appointment.API.Appointment.RescheduleAppointment
{
    public class RescheduleAppointmentEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/appointments/reschedule", async (
                RescheduleAppointmentCommand command,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
              
                var result = await mediator.Send(command, cancellationToken);

                return result
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("RescheduleAppointment")
            .WithTags("Appointments")
            .Produces(204)
            .ProducesProblem(400)
            .ProducesProblem(404)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Reschedule an appointment",
                Description = "Changes the date/time of an existing appointment"
            });
        }
    }
}

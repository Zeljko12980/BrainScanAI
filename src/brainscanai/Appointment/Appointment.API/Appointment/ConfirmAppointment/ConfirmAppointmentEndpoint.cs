namespace Appointment.API.Appointment.ConfirmAppointment
{
    public class ConfirmAppointmentEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/appointments/{id:guid}/confirm", async (
                Guid id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = new ConfirmAppointmentCommand(id);
                var result = await mediator.Send(command, cancellationToken);

                return result
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("ConfirmAppointment")
            .WithTags("Appointments")
            .Produces(204)
            .ProducesProblem(404)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Confirm an appointment",
                Description = "Marks a scheduled appointment as confirmed"
            });
        }
    }
}

namespace Appointment.API.Appointment.CancelAppointment
{
    public class CancelAppointmentEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/appointments/{id:guid}/cancel", async (
                Guid id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = new CancelAppointmentCommand(id);
                var result = await mediator.Send(command, cancellationToken);

                return result
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .WithName("CancelAppointment")
            .WithTags("Appointments")
            .Produces(204)
            .Produces(404);
           
        
        }
    }
}

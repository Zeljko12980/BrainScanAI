namespace Appointment.API.Appointment.GetAppointmentById
{
    public class GetAppointmentByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/appointments/{id:guid}", async (
                Guid id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAppointmentByIdQuery(id);
                var result = await mediator.Send(query, cancellationToken);

                return result is not null
                    ? Results.Ok(result)
                    : Results.NotFound();
            })
            .WithName("GetAppointmentById")
            .WithTags("Appointments")
            .Produces<AppointmentDto>()
            .ProducesProblem(404);
        }
    }
}

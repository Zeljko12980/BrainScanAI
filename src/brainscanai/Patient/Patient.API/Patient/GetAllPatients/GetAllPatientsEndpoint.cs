namespace Patient.API.Patient.GetAllPatients
{
    public class GetAllPatientsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/patients", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllPatientsQuery());
                return Results.Ok(result);
            });
        }
    }
}

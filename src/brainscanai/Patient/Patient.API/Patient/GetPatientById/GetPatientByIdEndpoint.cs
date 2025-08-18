namespace Patient.API.Patient.GetPatientById
{
    public class GetPatientByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/patients/{id:guid}", async (Guid id, IMediator sender) =>
            {
                var patient = await sender.Send(new GetPatientByIdQuery { Id = id });
                return patient is not null
                    ? Results.Ok(patient)
                    : Results.NotFound();
            });
        }
    }
}

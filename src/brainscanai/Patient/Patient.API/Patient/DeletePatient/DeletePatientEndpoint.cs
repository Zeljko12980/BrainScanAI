
using Patient.Application.Command;

namespace Patient.API.Patient.DeletePatient
{
    public class DeletePatientEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/patients/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeletePatientCommand { Id=id});
                return result
                    ? Results.NoContent()
                    : Results.NotFound();
            });
        }
    }
}

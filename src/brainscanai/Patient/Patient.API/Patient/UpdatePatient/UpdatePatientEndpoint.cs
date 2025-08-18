
using Patient.Application.Command;
using Patient.Application.Dtos;

namespace Patient.API.Patient.UpdatePatient
{
    public class UpdatePatientEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/patients/{id:guid}", async (Guid id, UpdatePatientDto dto, IMediator mediator) =>
            {
                var result = await mediator.Send(new UpdatePatientCommand(id, dto));
                return result ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}

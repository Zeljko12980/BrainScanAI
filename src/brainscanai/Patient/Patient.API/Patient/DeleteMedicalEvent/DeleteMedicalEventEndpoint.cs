
using BuildingBlocks.CQRS;
using Patient.Application.Command;

namespace Patient.API.Patient.DeleteMedicalEvent
{
    public class DeleteMedicalEventEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/patients/{patientId:guid}/medical-events/{eventId:guid}", async (Guid patientId, Guid eventId, IMediator mediator) =>
            {
                var command = new DeleteMedicalEventCommand
                {
                    PatientId = patientId,
                    EventId = eventId
                };

                var result = await mediator.Send(command);

                return result
                    ? Results.NoContent()
                    : Results.NotFound();
            });

        }
    }
}


using Patient.Application.Command;
using Patient.Application.Dtos;

namespace Patient.API.Patient.AddMedicalEvent
{
    public class AddMedicalEventEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/patients/{patientId:guid}/medical-events", async (
       Guid patientId,
       MedicalEventDto medicalEventDto,
       IMediator mediator) =>
            {
                var command = new AddMedicalEventCommand(patientId, medicalEventDto);
                var result = await mediator.Send(command);

                return result
                    ? Results.Ok("Medical event added successfully.")
                    : Results.BadRequest("Failed to add medical event.");
            });
        }
    }
}

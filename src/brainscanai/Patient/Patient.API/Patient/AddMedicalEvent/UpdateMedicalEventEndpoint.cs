using Patient.Application.Command;
using Patient.Application.Dtos;

namespace Patient.API.Patient.AddMedicalEvent
{
    public class UpdateMedicalEventEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/patients/{patientId:guid}/events/{eventId:guid}", async (
               Guid patientId,
               Guid eventId,
               MedicalEventDto updatedEvent,
               ISender sender,
               CancellationToken cancellationToken) =>
            {
                var command = new UpdateMedicalEventCommand(patientId, eventId, updatedEvent);
                var result = await sender.Send(command, cancellationToken);

                return result
                    ? Results.Ok(new { Message = "Medical event updated successfully." })
                    : Results.NotFound(new { Message = "Medical event or patient not found." });
            })
           .WithName("UpdateMedicalEvent")
           .WithTags("MedicalEvents")
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status404NotFound);
        }
    }
}

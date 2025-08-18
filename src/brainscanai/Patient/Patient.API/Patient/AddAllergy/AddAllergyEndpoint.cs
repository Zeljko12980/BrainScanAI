
using Patient.Application.Command;
using Patient.Domain.Enums;

namespace Patient.API.Patient.AddAllergy
{
    public record CreateAllergyRequest(string Name,Severity Severity, string? Notes);

    public class AddAllergyEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/patients/{patientId:guid}/allergies", async (
                Guid patientId,
                CreateAllergyRequest request,
                IMediator mediator) =>
            {
                var command = new AddAllergyCommand(
                    patientId,
                    request.Name,
                    request.Severity,
                    request.Notes
                );

                var success = await mediator.Send(command);

                return success
                    ? Results.Ok(new { Message = "Allergy added successfully." })
                    : Results.NotFound(new { Message = "Patient not found or invalid severity." });
            })
            .WithName("AddAllergy")
            .WithSummary("Adds a new allergy to a patient")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}

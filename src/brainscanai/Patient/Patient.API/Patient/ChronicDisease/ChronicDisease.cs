using Patient.Application.Command;
using Patient.Domain.Enums;

namespace Patient.API.Patient.ChronicDisease
{
    public record CreateChronicDiseaseRequest(string Name, Severity Severity, string? Notes);

    public class AddChronicDiseaseEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/patients/{patientId:guid}/chronic-diseases", async (
                Guid patientId,
                CreateChronicDiseaseRequest request,
                IMediator mediator) =>
            {
                var command = new AddChronicDiseaseCommand
                {
                    PatientId = patientId,
                    Name = request.Name,
                    Severity = request.Severity,
                    Notes = request.Notes
                };

                var success = await mediator.Send(command);

                return success
                    ? Results.Ok(new { Message = "Chronic disease added successfully." })
                    : Results.BadRequest(new { Message = "Failed to add chronic disease." });

            })
            .WithName("AddChronicDisease")
            .WithSummary("Adds a new chronic disease to a patient")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}

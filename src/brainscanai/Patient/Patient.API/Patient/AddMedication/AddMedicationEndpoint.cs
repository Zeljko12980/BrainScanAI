using Patient.Application.Command;

namespace Patient.API.Patient.AddMedication
{
    public record CreateMedicationRequest(string Name, string Dosage);

    public class AddMedicationEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/patients/{patientId:guid}/medications", async (
                Guid patientId,
                CreateMedicationRequest request,
                IMediator mediator) =>
            {
                var command = new AddMedicationCommand
                {
                    PatientId = patientId,
                    Name = request.Name,
                    Dosage = request.Dosage
                };

                var success = await mediator.Send(command);

                return success
                    ? Results.Ok(new { Message = "Medication added successfully." })
                    : Results.NotFound(new { Message = "Patient not found." });
            })
            .WithName("AddMedication")
            .WithSummary("Adds a new medication to a patient")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}

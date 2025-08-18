using Patient.Application.Command;

namespace Patient.API.Patient.AddScanImage
{
    public record CreateScanImageRequest(string ImageType, string Url, DateTime TakenAt);

    public class AddScanImageEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/patients/{patientId:guid}/scan-images", async (
                Guid patientId,
                CreateScanImageRequest request,
                IMediator mediator) =>
            {
                var command = new AddScanImageCommand
                {
                    PatientId = patientId,
                    ImageType = request.ImageType,
                    Url = request.Url,
                    TakenAt = request.TakenAt
                };

                var success = await mediator.Send(command);

                return success
                    ? Results.Ok(new { Message = "Scan image added successfully." })
                    : Results.NotFound(new { Message = "Patient not found or invalid data." });
            })
            .WithName("AddScanImage")
            .WithSummary("Adds a new scan image to a patient")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}

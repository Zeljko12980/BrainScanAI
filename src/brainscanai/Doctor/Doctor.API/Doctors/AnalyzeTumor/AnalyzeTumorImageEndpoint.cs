namespace Doctor.API.Doctors.AnalyzeTumor
{
  
    public record AnalyzeTumorImageRequest(string ImageBytes,Guid PatientId);

    public class AnalyzeTumorImageEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/tumor-analysis", async (
                [FromBody] AnalyzeTumorImageRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
               
                if (string.IsNullOrWhiteSpace(request.ImageBytes))
                {
                    return Results.BadRequest("Image bytes cannot be null or empty");
                }

                try
                {
                   
                    var command = new AnalyzeTumorImageCommand(request.ImageBytes,request.PatientId);
                    var (tumorType, confidence) = await mediator.Send(command, cancellationToken);

                   
                    return Results.Ok(new
                    {
                        TumorType = tumorType,
                        Confidence = confidence,
                        AnalysisDate = DateTime.UtcNow
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        title: "Tumor analysis failed",
                        detail: ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .Accepts<AnalyzeTumorImageRequest>("application/json") 
            .Produces(StatusCodes.Status200OK, contentType: "application/json")
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("AnalyzeTumorImage")
            .WithTags("Tumor Analysis")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Analyze tumor image",
                Description = "Performs analysis on a tumor image and returns the predicted tumor type with confidence score"
            });
        }
    }
}

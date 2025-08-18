
namespace Patient.API.Patient.MedicalHistory
{
    public class GetMedicalHistoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/patients/{id:guid}/medical-history", async(Guid id,IMediator sender) =>
            {
                var result = await sender.Send(new MedicalHistoryQuery() { Id=id});

                return result;
            });
        }
    }
}


using Doctor.Application.Dtos;
using Doctor.Application.Query;

namespace Doctor.API.Doctors.GetDoctor
{
    public class GetDoctorByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.Map("/api/doctors/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var query = new GetDoctorByIdQuery { Id = id };
                var doctor = await mediator.Send(query);

                return doctor is not null
                    ? Results.Ok(doctor)
                    : Results.NotFound();
            })
                  .WithName("GetDoctorById")
        .Produces<DoctorDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithDescription("Gets a doctor by their unique identifier")
        .WithDisplayName("Get Doctor by ID"); 
        }
    }
}

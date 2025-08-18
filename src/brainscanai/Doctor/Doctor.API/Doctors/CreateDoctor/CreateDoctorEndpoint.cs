using Carter;
using Doctor.Application.Command;
using Doctor.Application.Dtos;
using MediatR;

namespace Doctor.API.Doctors.CreateDoctor
{
    public class CreateDoctorEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/doctors", async (CreateDoctorCommand command, ISender sender) =>
            {
                var result = await sender.Send(command);
                return Results.Created($"/api/doctors/{result.Id}", result);
            })
            .WithName("CreateDoctor")
            .WithTags("Doctors")
            .Produces<DoctorDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}

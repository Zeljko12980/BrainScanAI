using Microsoft.AspNetCore.Authorization;
using Patient.Application.Command;
using Patient.Application.Dtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Patient.API.Patient.CreatePatient
{
    public class CreatePatientEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/patients",
         
            async (
            CreatePatientDto dto,
            HttpContext context,
            ISender mediator) =>
            {
                var doctorIdClaim = "b760a3a2-d636-407a-8985-d9ae6679bc0b";
                if (doctorIdClaim == null || !Guid.TryParse(doctorIdClaim, out var doctorId))
                {
                    return Results.Unauthorized();
                }
                var result = await mediator.Send(new CreatePatientCommand(dto,doctorId));
                return Results.Created($"/api/patients/{result.Id}", result);
            })
         
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);
        }
    }
}

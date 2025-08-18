using Auth.Application.Commands.Role;

namespace Auth.API.Auth.CreateRole
{
    public class CreateRoleEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/role", async (
               [FromBody] RoleCreateCommand command,
               ISender sender) =>
            {
                var result = await sender.Send(command);

                if (result == 1)
                    return Results.Ok(new { Message = "Role created successfully." });

                return Results.BadRequest(new { Message = "Failed to create role." });
            })
           .WithName("CreateRole")
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest);
        }
    }
}

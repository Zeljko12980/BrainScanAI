namespace Auth.API.Auth.Register
{
    /*
    public class RegisterEndpoint : ICarterModule
    {
        
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/auth/register", async (RegisterUserCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result.isSucceed
                    ? Results.Ok(new { message = "Registration successful", userId = result.userId })
                    : Results.BadRequest(new { message = "Registration failed" });
            })
             .WithName("RegisterUser")
             .WithSummary("Registers a new user")
             .WithDescription("Registers a new user with provided username, password, email, first name, and last name. Returns user ID on success.")
             .Produces(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status400BadRequest);

        }
    }
    */
}

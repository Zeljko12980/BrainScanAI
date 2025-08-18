namespace Auth.API.Auth.Login
{
    public class LoginEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/auth/login", async ([FromBody] AuthCommand command, IMediator mediator) =>
            {
                await mediator.Send(command);
                return Results.Ok("2FA code sent.");
            })
            .WithName("LoginUser")
            .WithSummary("Initiates the login process")
            .WithDescription("Validates the user credentials and sends a 2FA code to the registered contact method (email or phone).")
            .Produces(StatusCodes.Status200OK, typeof(string))
            .Produces(StatusCodes.Status400BadRequest, typeof(string));
        }
    }
}

namespace Auth.API.Auth.Verify
{
    public class VerifyEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/auth/verify", async ([FromBody] VerifyTwoFactorCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result;
            })
            .WithName("Verify2FA")
            .WithSummary("Verifies 2FA code")
            .WithDescription("Validates the 2-factor authentication (2FA) code for a user. This endpoint is typically called after a successful login to confirm the user's identity.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        }
    }
}

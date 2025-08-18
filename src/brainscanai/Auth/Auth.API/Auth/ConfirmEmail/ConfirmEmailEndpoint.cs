namespace Auth.API.Auth.ConfirmEmail
{
    public class ConfirmEmailEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/auth/confirm-email", async ([FromQuery] string userId, [FromQuery] string token, IMediator mediator) =>
            {
                var result = await mediator.Send(new ConfirmEmailCommand(userId, token));
                return result ? Results.Ok("Email confirmed successfully.") : Results.BadRequest("Confirmation failed.");
            })
            .WithName("ConfirmEmail")
            .WithSummary("Confirms a user's email address")
            .WithDescription("Confirms the user's email address using the user ID and confirmation token provided in the query parameters.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        }
    }
}
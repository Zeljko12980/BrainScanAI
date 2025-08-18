namespace Notification.API.Notifications.CreateNotification
{
    public class CreateNotificationEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/notifications", async ([FromBody]CreateNotificationCommand command,
                IMediator sender) =>
            {
                try
                {
                    var result = await sender.Send(command);
                    return Results.Created($"/api/notifications/{result.Id}", result);
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        title: "Error creating notification",
                        detail: ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithTags("Notifications")
            .Produces<NotificationDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
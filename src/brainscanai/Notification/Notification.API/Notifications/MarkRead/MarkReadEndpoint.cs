namespace Notification.API.Notifications.MarkAsRead
{
    
    public class MarkNotificationAsReadEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/notifications/{notificationId:guid}/read", async (
                Guid notificationId,
                IMediator sender,
                CancellationToken cancellationToken) =>
            {
                var command = new MarkNotificationAsReadCommand(notificationId);
                var result = await sender.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .WithTags("Notifications")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            ;
        }
    }
   
}
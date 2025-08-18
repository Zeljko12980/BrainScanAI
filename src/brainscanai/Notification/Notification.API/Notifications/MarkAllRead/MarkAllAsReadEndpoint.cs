namespace Notification.API.Notifications.MarkAllAsRead
{
    public class MarkAllNotificationsAsReadEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPatch("/api/notifications/mark-all-read", async (
                [FromBody] MarkAllNotificationsAsReadRequest request,
                IMediator sender,
                CancellationToken cancellationToken) =>
            {
                
             
                var command = new MarkAllNotificationsAsReadCommand(request.UserId);
                await sender.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .WithTags("Notifications")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }

    public record MarkAllNotificationsAsReadRequest(
        [Required(ErrorMessage = "UserId is required")]
        [StringLength(36, ErrorMessage = "UserId must be 36 characters")]
        string UserId);
}

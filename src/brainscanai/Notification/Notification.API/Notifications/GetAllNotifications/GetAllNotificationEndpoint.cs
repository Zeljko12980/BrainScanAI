using Notification.Application.Query;

namespace Notification.API.Notifications.GetAllByUser
{
    public class GetAllNotificationsByUserIdEndpoint : ICarterModule
    {
        
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users/{userId}/notifications", async (
                string userId,
                IMediator sender,
             
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllNotificationsByUserIdQuery(userId);

               

                var result = await sender.Send(query, cancellationToken);
                return Results.Ok(result);
            })
            .WithTags("Notifications")
            .Produces<IEnumerable<NotificationDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
        
}
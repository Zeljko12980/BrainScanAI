
namespace Notification.Application.Query
{
  public record GetAllNotificationsByUserIdQuery(
      string UserId
      ):IQuery<IEnumerable<NotificationDto>>;

    public class GetAllNotificationsByUserIdQueryHandler
        (INotificationService notificationService)
        : IQueryHandler<GetAllNotificationsByUserIdQuery, IEnumerable<NotificationDto>>
    {
        public async Task<IEnumerable<NotificationDto>> Handle(GetAllNotificationsByUserIdQuery query, CancellationToken cancellationToken)
        {
            var result = await notificationService.GetUserNotificationsAsync(query.UserId);
            return result;
        }
    }
}

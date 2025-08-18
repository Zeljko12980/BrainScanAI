namespace Notification.Application.Common.Interface
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId);
        Task MarkAllAsReadAsync(string userId);
        Task MarkAsReadAsync(Guid notificationId);
        Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto notificationDto);
    }
}

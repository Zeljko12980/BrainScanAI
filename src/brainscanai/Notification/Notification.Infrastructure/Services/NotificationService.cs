using Microsoft.AspNetCore.SignalR;
using Notification.Infrastructure.Hubs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Notification.Infrastructure.Services
{
    public class NotificationService
        (NotificationDbContext context, IMapper mapper, IHubContext<NotificationHub> hubContext)
        : INotificationService
    {
        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId)
        {
            var notification= await context.Notifications
          .Where(n => n.UserId == userId)
          .OrderByDescending(n => n.CreatedAt)
          .Take(50) 
          .ToListAsync();

            return mapper.Map<IEnumerable<NotificationDto>>(notification);   
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            var notifications = await context.Notifications
          .Where(n => n.UserId == userId && !n.IsRead)
          .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(Guid notificationId)
        {
            var notification = await context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await context.SaveChangesAsync();
            }
        }

        public async Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto notificationDto)
        {
            var notification = new Domain.Entities.Notification
            {
                Id = Guid.NewGuid(),
                UserId = notificationDto.UserId,
                Title = notificationDto.Title,
                Message = notificationDto.Message,
                Icon = notificationDto.Icon,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            context.Notifications.Add(notification);
            await context.SaveChangesAsync();
;

            if (NotificationHub.Connections.TryGetValue(notification.UserId, out var connectionId))
            {
                await hubContext.Clients.Client(connectionId)
                    .SendAsync("ReceiveNotification", notification);
            }



            return mapper.Map<NotificationDto>(notification);
        }

    }
}

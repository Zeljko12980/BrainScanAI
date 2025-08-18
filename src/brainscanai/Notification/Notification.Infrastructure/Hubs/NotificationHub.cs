using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Hubs
{
    [Authorize] 
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            Console.WriteLine($"User connected: {userId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            Console.WriteLine($"User disconnected: {userId}");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotificationToUser(string targetUserId, Notification.Domain.Entities.Notification notification)
        {
            await Clients.User(targetUserId).SendAsync("ReceiveNotification", notification);
        }
    }
}

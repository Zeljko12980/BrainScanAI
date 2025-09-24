using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Appointment.Infrastructure.Hubs
{
    [AllowAnonymous]
    public class AppointmentHub : Hub
    {
        public static readonly Dictionary<string, string> Connections = new();

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"New connection: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Disconnected: {Context.ConnectionId}");

            var user = Connections.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (!string.IsNullOrEmpty(user.Key))
            {
                Connections.Remove(user.Key);
                Console.WriteLine($"User {user.Key} removed from connections");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public Task RegisterUserAsync(string userId)
        {
            Connections[userId] = Context.ConnectionId;
            Console.WriteLine($"User {userId} registered with connection {Context.ConnectionId}");
            return Task.CompletedTask;
        }

        public async Task SendToUserAsync(string userId, AppointmentDto appointment)
        {
            if (Connections.TryGetValue(userId, out var connectionId))
            {
                await Clients.Client(connectionId)
                    .SendAsync("ReceiveAppointment", appointment);
            }
        }
    }
}

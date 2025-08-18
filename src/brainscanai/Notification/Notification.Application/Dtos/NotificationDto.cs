namespace Notification.Application.Dtos
{
    public class NotificationDto
    {
        public Guid Id { get; init; }
        public string UserId { get; init; }
        public string Title { get; init; }
        public string Message { get; init; }
        public string Icon { get; init; }
        public DateTime CreatedAt { get; init; }
        public bool IsRead { get; init; }
        public string TimeAgo { get; init; }

        // Computed property example
        public string FormattedDate => CreatedAt.ToString("MMMM dd, yyyy HH:mm");
    }
}

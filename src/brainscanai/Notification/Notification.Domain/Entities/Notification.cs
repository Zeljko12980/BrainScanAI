namespace Notification.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }  
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string Icon { get; set; } 

   
        public string TimeAgo => GetTimeAgoString();

        private string GetTimeAgoString()
        {
            var timeSpan = DateTime.UtcNow - CreatedAt;
            return timeSpan.TotalSeconds switch
            {
                < 60 => "Just now",
                < 3600 => $"{(int)timeSpan.TotalMinutes} minutes ago",
                < 86400 => $"{(int)timeSpan.TotalHours} hours ago",
                _ => CreatedAt.ToString("MMMM dd, yyyy")
            };
        }
    }
}

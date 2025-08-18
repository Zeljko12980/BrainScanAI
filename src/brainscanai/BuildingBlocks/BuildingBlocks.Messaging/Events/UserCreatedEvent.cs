

namespace BuildingBlocks.Messaging.Events
{
    public record UserCreatedEvent:IntegrationEvent
    {
        public Guid UserId { get; set; }  

        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public string ProfilePictureUrl { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}

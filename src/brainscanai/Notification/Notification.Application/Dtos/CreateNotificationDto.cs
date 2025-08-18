namespace Notification.Application.Dtos
{
    public class CreateNotificationDto
    {
        public string UserId { get; set; }
        public string Title {  get; set; }
        public string Message {  get; set; }
        public string Icon {  get; set; }
    }
}

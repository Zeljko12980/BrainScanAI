namespace Notification.Application.Command
{
    public record CreateNotificationCommand(
     string UserId,
     string Title,
     string Message,
     string Icon):ICommand<NotificationDto>;

    public class CreateNotificationCommandHandler
        (INotificationService notificationService)
        : ICommandHandler<CreateNotificationCommand, NotificationDto>
    {
        public async Task<NotificationDto> Handle(CreateNotificationCommand command, CancellationToken cancellationToken)
        {
            var result = await notificationService.CreateNotificationAsync(new CreateNotificationDto()
            {
                UserId = command.UserId,
                Title = command.Title,
                Message = command.Message,
                Icon = command.Icon
            });

            return result;
        }
    }
}

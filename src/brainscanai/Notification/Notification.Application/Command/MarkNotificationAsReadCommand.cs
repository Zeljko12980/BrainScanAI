namespace Notification.Application.Command
{
    public record MarkNotificationAsReadCommand(Guid NotificationId):ICommand<Unit>;

    public class MarkNotificationAsReadCommandHandler
        (INotificationService notificationService)
        : ICommandHandler<MarkNotificationAsReadCommand, Unit>
    {
        public async Task<Unit> Handle(MarkNotificationAsReadCommand command, CancellationToken cancellationToken)
        {
            await notificationService.MarkAsReadAsync(command.NotificationId);

            return Unit.Value;
        }
    }
}

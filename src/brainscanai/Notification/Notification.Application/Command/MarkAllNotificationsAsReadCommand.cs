namespace Notification.Application.Command
{
    public record MarkAllNotificationsAsReadCommand(string UserId):ICommand<Unit>;

    public class MarkAllNotificationsAsReadCommandHandler
        (INotificationService notificationService)
        : ICommandHandler<MarkAllNotificationsAsReadCommand, Unit>
    {
        public async Task<Unit> Handle(MarkAllNotificationsAsReadCommand command, CancellationToken cancellationToken)
        {
            await notificationService.MarkAllAsReadAsync(command.UserId);

            return Unit.Value;
        }
    }
}

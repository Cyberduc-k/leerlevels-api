using Model;

namespace Service.Interfaces;

public interface INotificationService
{
    /**
     * <summary>
     * Send a notification.
     * </summary>
     * <remarks>
     * <see cref="Notification"/> will send a notificaton to all users.<br/>
     * <see cref="PersonalNotification"/> will send a notification to a specific user.
     * </remarks>
     */
    public Task SendNotificationAsync(Notification notification);
}

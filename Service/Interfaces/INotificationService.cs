using Model;

namespace Service.Interfaces;

public interface INotificationService
{
    public Task SendNotificationAsync(Notification notification);
}

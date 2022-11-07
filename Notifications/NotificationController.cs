using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service;
using Service.Interfaces;

namespace Notifications;

public class NotificationController
{
    private readonly ILogger _logger;
    private readonly IUserService _userService;

    public NotificationController(ILoggerFactory loggerFactory, IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<NotificationController>();
        _userService = userService;
    }

    [Function(nameof(SendNotification))]
    public async Task SendNotification(
        [QueueTrigger(NotificationService.QUEUE_NAME)] string item)
    {
        _logger.LogInformation($"send notification: {item}");
        string connectionString = Environment.GetEnvironmentVariable("LeerLevelsNotificationHub")!;
        string hubName = Environment.GetEnvironmentVariable("LeerLevelsNotificationHubName")!;
        NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, hubName);
        Model.PersonalNotification? pn = JsonConvert.DeserializeObject<Model.PersonalNotification>(item);
        Model.Notification n = JsonConvert.DeserializeObject<Model.Notification>(item)!;
        Notification[] notifications = new Notification[] {
            new FcmNotification($"{{\"notification\":{{\"title\":\"{n.Title}\",\"body\":\"{n.Message}\"}}}}"),
            new AppleNotification($"{{\"aps\":{{\"alert\":\"{n.Message}\"}}}}"),
        };

        if (pn is not null) {
            Model.User user = await _userService.GetUserById(pn.UserId);

            foreach (Notification notification in notifications) {
                NotificationOutcome result = await hub.SendDirectNotificationAsync(notification, user.LastDeviceHandle);

                _logger.LogInformation($"notification sent to: {user.UserName} ({result.Success} devices)");
            }
        } else {
            foreach (Notification notification in notifications) {
                NotificationOutcome result = await hub.SendNotificationAsync(notification);

                _logger.LogInformation($"notification sent to {result.Success} devices");
            }
        }
    }
}

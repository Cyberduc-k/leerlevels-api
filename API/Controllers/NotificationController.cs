using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API.Controllers;

public class NotificationController : ControllerBase
{
    public NotificationController(ILoggerFactory loggerFactory)
        : base(loggerFactory.CreateLogger<NotificationController>())
    {
    }

    [Function(nameof(SendNotification))]
    public async Task SendNotification(
        [QueueTrigger("notifications")] string item)
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

        if (pn is Model.PersonalNotification _pn) {
            throw new NotImplementedException("personal notifications");
        } else {
            foreach (Notification notification in notifications) {
                NotificationOutcome result = await hub.SendNotificationAsync(notification);

                _logger.LogInformation($"notification sent to: {result.Success}");
            }
        }
    }

    [Function(nameof(TestNotifications))]
    [QueueOutput("notifications")]
    public async Task<string> TestNotifications(
        [TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timer)
    {
        Model.Notification n = new("Test title", "Test message");

        return JsonConvert.SerializeObject(n);
    }
}

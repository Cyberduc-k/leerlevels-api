using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;

namespace API.Controllers;

public class NotificationController
{
    private readonly ILogger _logger;

    public NotificationController(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<NotificationController>();
    }

    [Function(nameof(SendNotification))]
    public async Task SendNotification(
        [QueueTrigger("notifications")] string item)
    {
        _logger.LogInformation($"send notification: {item}");
        string connectionString = Environment.GetEnvironmentVariable("LeerLevelsNotificationHub")!;
        string hubName = Environment.GetEnvironmentVariable("LeerLevelsNotificationHubName")!;
        NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, hubName);
        Notification[] notifications = new Notification[] {
            new FcmNotification(item),
            new AppleNotification(item),
        };

        foreach (Notification notification in notifications) {
            NotificationOutcome result = await hub.SendNotificationAsync(notification);

            _logger.LogInformation($"notification sent to: {result.Success}");
        }
    }

    [Function(nameof(TestNotifications))]
    [QueueOutput("notifications")]
    public async Task<string> TestNotifications(
        [TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timer)
    {
        return "test";
    }
}

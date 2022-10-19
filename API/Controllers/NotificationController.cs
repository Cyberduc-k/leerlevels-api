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
        [TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timer)
    {
        string connectionString = Environment.GetEnvironmentVariable("LeerLevelsNotificationHub")!;
        string hubName = Environment.GetEnvironmentVariable("LeerLevelsNotificationHubName")!;
        NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, hubName);

        string fmcNot = "{\"data\":{\"body\":\"test body\",\"title\":\"test title\"}}";
        NotificationOutcome result = await hub.SendFcmNativeNotificationAsync(fmcNot);

        _logger.LogInformation($"notification sent to: {result.Success}");
    }
}

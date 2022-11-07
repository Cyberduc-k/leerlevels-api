using Azure.Storage.Queues;
using Model;
using Newtonsoft.Json;
using Service.Interfaces;

namespace Service;

public class NotificationService : INotificationService
{
    public const string QUEUE_NAME = "notifications";
    private readonly QueueClient _queueClient;

    public NotificationService()
    {
        string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage")!;

        _queueClient = new QueueClient(connectionString, QUEUE_NAME, new() { MessageEncoding = QueueMessageEncoding.Base64 });
    }

    //! ONLY USE FOR TESTING
    public NotificationService(QueueClient queueClient)
    {
        _queueClient = queueClient;
    }

    public async Task SendNotificationAsync(Notification notification)
    {
        string json = JsonConvert.SerializeObject(notification);

        await _queueClient.SendMessageAsync(json);
    }
}

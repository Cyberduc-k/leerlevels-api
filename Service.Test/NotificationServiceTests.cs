using Azure.Storage.Queues;
using Model;
using Moq;
using Xunit;

namespace Service.Test;

public class NotificationServiceTests
{
    private readonly Mock<QueueClient> _queue;
    private readonly NotificationService _notificationService;

    public NotificationServiceTests()
    {
        _queue = new();
        _notificationService = new(_queue.Object);
    }

    [Fact]
    public async Task Send_Notification_Should_Return()
    {
        _queue.Setup(q => q.SendMessageAsync(It.IsAny<string>())).Verifiable();
        await _notificationService.SendNotificationAsync(new Notification());
        _queue.Verify(q => q.SendMessageAsync(It.IsAny<string>()), Times.Once);
    }
}

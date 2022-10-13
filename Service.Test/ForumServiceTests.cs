using Microsoft.Extensions.Logging;
using Model;
using Moq;
using Repository.Interfaces;
using Service.Exceptions;
using Xunit;

namespace Service.Test;

public class ForumServiceTests
{
    private readonly Mock<IForumRepository> _mockRepository;
    private readonly Mock<IForumReplyRepository> _mockReplyRepository;
    private readonly ForumService _service;

    public ForumServiceTests()
    {
        _mockRepository = new();
        _mockReplyRepository = new();
        _service = new ForumService(new LoggerFactory(), _mockRepository.Object, _mockReplyRepository.Object);
    }

    [Fact]
    public async Task GetByIdReturnsForum()
    {
        _mockRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => new Forum("1", "Test Forum", "test test test", null!, null!));
        Forum forum = await _service.GetById("1");

        Assert.Equal("1", forum.Id);
    }

    [Fact]
    public void GetByIdThrowsNotFoundException()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);

        Assert.ThrowsAsync<NotFoundException>(() => _service.GetById("fdsfdfs"));
    }
}

using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
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
    public async Task GetAllReturnsArray()
    {
        async IAsyncEnumerable<Forum> MockForums()
        {
            yield return new Forum("1", "Test Forum 1", "test", null!, null!);
            yield return new Forum("2", "Test Forum 2", "test test", null!, null!);
        }

        _mockRepository.Setup(r => r.GetAllAsync()).Returns(() => MockForums());

        ICollection<Forum> forums = await _service.GetAll();

        Assert.Equal(2, forums.Count);
    }

    [Fact]
    public async Task CreateForumReturnsForum()
    {
        _mockRepository.Setup(r => r.InsertAsync(It.IsAny<Forum>())).Verifiable();
        _mockRepository.Setup(r => r.SaveChanges()).Verifiable();

        Forum newForum = new();
        Forum forum = await _service.CreateForum(newForum);

        Assert.NotNull(forum.Id);

        _mockRepository.Verify(r => r.InsertAsync(It.IsAny<Forum>()), Times.Once);
        _mockRepository.Verify(r => r.SaveChanges(), Times.Once);
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

    [Fact]
    public async Task GetReplyByIdReturnsForumReply()
    {
        _mockReplyRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => new ForumReply("1", null!, "Test Forum", 0));
        ForumReply reply = await _service.GetReplyById("1");

        Assert.Equal("1", reply.Id);
    }

    [Fact]
    public void GetReplyByIdThrowsNotFoundException()
    {
        _mockReplyRepository.Setup(r => r.GetByIdAsync(It.IsNotIn("1"))).ReturnsAsync(() => null);

        Assert.ThrowsAsync<NotFoundException>(() => _service.GetReplyById("fdsfdfs"));
    }

    [Fact]
    public async Task UpdateForumHasPropertyChanged()
    {
        Forum forum = new("1", "Test Title", "test test test", null!, null!);

        _mockRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => forum);
        _mockRepository.Setup(r => r.SaveChanges()).Verifiable();

        UpdateForumDTO changes = new() { Title = "New Test Title" };
        await _service.UpdateForum("1", changes);

        Assert.Equal("New Test Title", forum.Title);
        _mockRepository.Verify(r => r.SaveChanges(), Times.Once);
    }
}

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

    [Fact]
    public async Task UpdateForumThrowsNotFoundException()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("forum post"));

        UpdateForumDTO changes = new() { Title = "New Test Title" };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateForum("1", changes));
    }

    [Fact]
    public async Task AddReplyReturnsReply()
    {
        Forum forum = new("1", "Test Forum", "test test test", null!, new List<ForumReply>());

        _mockReplyRepository.Setup(r => r.InsertAsync(It.IsAny<ForumReply>())).Verifiable();
        _mockRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => forum);
        _mockRepository.Setup(r => r.SaveChanges()).Verifiable();

        ForumReply newReply = new("1", null!, "test reply", 0);
        ForumReply reply = await _service.AddReply("1", newReply);

        Assert.NotNull(reply.Id);
        Assert.Equal(1, forum.Replies.Count);

        _mockReplyRepository.Verify(r => r.InsertAsync(It.IsAny<ForumReply>()), Times.Once);
        _mockRepository.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public void AddReplyThrowsNotFoundException()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("forum post"));

        ForumReply newReply = new("1", null!, "test reply", 0);

        Assert.ThrowsAsync<NotFoundException>(() => _service.AddReply("1", newReply));
    }

    [Fact]
    public async Task UpdateForumReplyHasPropertyChanged()
    {
        ForumReply reply = new("1", null!, "test test test", 0);

        _mockReplyRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => reply);
        _mockReplyRepository.Setup(r => r.SaveChanges()).Verifiable();

        UpdateForumReplyDTO changes = new() { Text = "test test" };
        await _service.UpdateForumReply("1", changes);

        Assert.Equal("test test", reply.Text);
        _mockReplyRepository.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public void UpdateForumReplyThrowsNotFoundException()
    {
        _mockReplyRepository.Setup(r => r.GetByIdAsync(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("forum post reply"));

        UpdateForumReplyDTO changes = new() { Text = "test test" };

        Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateForumReply("1", changes));
    }

    [Fact]
    public async Task DeleteForumReplyDeletesIt()
    {
        ForumReply reply = new("1", null!, "test test", 0);
        Forum forum = new("1", "Test Title", "Test Description", null!, new List<ForumReply>() { reply });

        _mockReplyRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => reply);
        _mockReplyRepository.Setup(r => r.RemoveAsync("1")).Verifiable();
        _mockRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => forum);
        _mockRepository.Setup(r => r.SaveChanges()).Verifiable();

        await _service.DeleteForumReply("1", "1");

        Assert.DoesNotContain(reply, forum.Replies);

        _mockReplyRepository.Verify(r => r.RemoveAsync("1"), Times.Once);
        _mockRepository.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public void DeleteForumReplyThrowsForumNotFoundException()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("forum post"));

        Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteForumReply("1", "1"));
    }

    [Fact]
    public void DeleteForumReplyThrowsForumReplyNotFoundException()
    {
        _mockRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => new Forum("1", "Test Forum", "test test test", null!, null!));
        _mockReplyRepository.Setup(r => r.GetByIdAsync(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("forum post reply"));

        Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteForumReply("1", "1"));
    }
}

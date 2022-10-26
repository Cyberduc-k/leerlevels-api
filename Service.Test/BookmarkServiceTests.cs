using MockQueryable.Moq;
using Model;
using Moq;
using Repository.Interfaces;
using Xunit;

namespace Service.Test;

public class BookmarkServiceTests
{
    private readonly Mock<IBookmarkRepository> _bookmarkRepository;
    private readonly Mock<ITargetRepository> _targetRepository;
    private readonly Mock<IMcqRepository> _mcqRepository;
    private readonly User _user;
    private readonly BookmarkService _service;

    public BookmarkServiceTests()
    {
        _bookmarkRepository = new();
        _targetRepository = new();
        _mcqRepository = new();
        _user = new() { Id = "1" };
        _service = new BookmarkService(_bookmarkRepository.Object, _targetRepository.Object, _mcqRepository.Object);

        _targetRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => new Target());
        _mcqRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(() => new Mcq());
    }

    [Fact]
    public async Task Get_Bookmarks_Should_Return_An_Array_Of_Targets_And_An_Array_Of_Mcqs()
    {
        Bookmark[] mockBookmarks = new[] {
            new Bookmark("1", Bookmark.BookmarkType.Target) { UserId = "1" },
            new Bookmark("1", Bookmark.BookmarkType.Mcq) { UserId = "1" },
        };

        _bookmarkRepository.Setup(r => r.GetAllAsync()).Returns(mockBookmarks.BuildMock());

        (ICollection<Target> targets, ICollection<Mcq> mcqs) = await _service.GetBookmarksAsync(_user);

        Assert.Equal(1, targets.Count);
        Assert.Equal(1, mcqs.Count);
    }

    [Fact]
    public async Task Add_Bookmark_Should_Add_A_Bookmark_To_User()
    {
        _bookmarkRepository.Setup(r => r.InsertAsync(It.IsAny<Bookmark>())).Verifiable();
        _bookmarkRepository.Setup(r => r.SaveChanges()).Verifiable();

        await _service.AddBookmark(_user, new Bookmark("2", Bookmark.BookmarkType.Target));

        _bookmarkRepository.Verify(r => r.InsertAsync(It.IsAny<Bookmark>()), Times.Once);
        _bookmarkRepository.Verify(r => r.SaveChanges(), Times.Once);
    }
}

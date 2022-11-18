using System.Net;
using API.Controllers;
using API.Test.Mock;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Model.Response;
using Moq;
using Service.Interfaces;
using Xunit;

namespace API.Test;

public class BookmarkControllerTests : ControllerTestsBase
{
    private readonly Mock<IBookmarkService> _bookmarkService;
    private readonly BookmarkController _controller;

    public BookmarkControllerTests()
    {
        _bookmarkService = new();
        _controller = new(new LoggerFactory(), _tokenService.Object, _mapper, _bookmarkService.Object, _userService.Object);

        _bookmarkService
            .Setup(s => s.GetBookmarksAsync(It.IsAny<User>()))
            .ReturnsAsync(() => (
                new Target[] { new() { Id = "1", Label = "Label", Description = "Description" } },
                new Mcq[] { new() { Id = "1", QuestionText = "QuestionText" } }
            ));
    }

    [Fact]
    public async Task Get_Bookmarks_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetBookmarks(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_Bookmarks_Should_Respond_Json()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetBookmarks(request);
        BookmarksResponse? result = await response.ReadFromJsonAsync<BookmarksResponse>();

        Assert.Equal("application/json; charset=utf-8", GetHeaderValue(response, "Content-Type"));
        Assert.NotNull(result);
        Assert.Equal(1, result!.Targets.Count);
        Assert.Equal(1, result!.Mcqs.Count);
    }

    [Fact]
    public async Task Add_Bookmark_Should_Respond_OK()
    {
        _bookmarkService.Setup(s => s.AddBookmark(It.IsAny<User>(), It.IsAny<Bookmark>())).Verifiable();

        HttpRequestData request = MockHelpers.CreateHttpRequestData(new BookmarkDTO("2", Bookmark.BookmarkType.Mcq));
        HttpResponseData response = await _controller.AddBookmark(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _bookmarkService.Verify(s => s.AddBookmark(It.IsAny<User>(), It.IsAny<Bookmark>()), Times.Once);
    }
}

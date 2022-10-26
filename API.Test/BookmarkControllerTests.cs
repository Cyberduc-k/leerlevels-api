using System.Net;
using API.Controllers;
using API.Mappings;
using API.Test.Mocks;
using AutoMapper;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Model;
using Moq;
using Service.Interfaces;
using Xunit;

namespace API.Test;

public class BookmarkControllerTests
{
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<IBookmarkService> _bookmarkService;
    private readonly BookmarkController _controller;

    public BookmarkControllerTests()
    {
        Mapper mapper = new(new MapperConfiguration(c => c.AddProfile<MappingProfile>()));

        _tokenService = new();
        _bookmarkService = new();
        _controller = new(new LoggerFactory(), _tokenService.Object, mapper, _bookmarkService.Object);

        // authenticate all requests.
        _tokenService.Setup(s => s.AuthenticationValidation(It.IsAny<HttpRequestData>())).ReturnsAsync(() => true);
        _tokenService.SetupGet(s => s.User).Returns(new User() { Id = "1", Role = UserRole.Student });
    }

    [Fact]
    public async Task Get_Bookmarks_Should_Respond_OK()
    {
        _bookmarkService
            .Setup(s => s.GetBookmarksAsync(It.IsAny<User>()))
            .ReturnsAsync(() => (Array.Empty<Target>(), Array.Empty<Mcq>()));

        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetBookmarks(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

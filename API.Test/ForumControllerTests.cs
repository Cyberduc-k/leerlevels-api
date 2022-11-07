using System.Net;
using API.Controllers;
using API.Test.Mock;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Model.Response;
using Moq;
using Service.Exceptions;
using Service.Interfaces;
using Xunit;

namespace API.Test;

public class ForumControllerTests : ControllerTestsBase
{
    private readonly Mock<IForumService> _forumService;
    private readonly ForumController _controller;

    public ForumControllerTests()
    {
        _forumService = new();
        _controller = new(new LoggerFactory(), _tokenService.Object, _mapper, _forumService.Object);

        Forum forum = new() { Id = "1", Title = "Title", Description = "Description", From = User };
        ForumReply forumReply = new() { Id = "1", Text = "Text", Upvotes = 0, From = User };

        _forumService.Setup(s => s.GetAll()).ReturnsAsync(() => new Forum[] { forum });
        _forumService.Setup(s => s.GetById("1")).ReturnsAsync(() => forum);
        _forumService.Setup(s => s.GetById(It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("forum post"));
        _forumService.Setup(s => s.CreateForum(It.IsAny<Forum>())).Verifiable();
        _forumService.Setup(s => s.AddReply("1", It.IsAny<ForumReply>())).ReturnsAsync(() => forumReply);
        _forumService.Setup(s => s.AddReply(It.IsNotIn("1"), It.IsAny<ForumReply>())).ThrowsAsync(new NotFoundException("forum post"));
        _forumService.Setup(s => s.UpdateForum("1", It.IsAny<UpdateForumDTO>())).Verifiable();
        _forumService.Setup(s => s.UpdateForum(It.IsNotIn("1"), It.IsAny<UpdateForumDTO>())).ThrowsAsync(new NotFoundException("forum post"));
        _forumService.Setup(s => s.UpdateForumReply("1", It.IsAny<UpdateForumReplyDTO>())).Verifiable();
        _forumService.Setup(s => s.UpdateForumReply(It.IsNotIn("1"), It.IsAny<UpdateForumReplyDTO>())).ThrowsAsync(new NotFoundException("forum post reply"));
        _forumService.Setup(s => s.DeleteForumReply("1", "1")).Verifiable();
        _forumService.Setup(s => s.DeleteForumReply(It.IsNotIn("1"), It.IsAny<string>())).ThrowsAsync(new NotFoundException("forum post"));
        _forumService.Setup(s => s.DeleteForumReply("1", It.IsNotIn("1"))).ThrowsAsync(new NotFoundException("forum post"));
    }

    [Fact]
    public async Task Get_Forums_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetForums(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_Forums_Should_Respond_Json()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetForums(request);
        ICollection<ForumResponse>? result = await response.ReadFromJsonAsync<ForumResponse[]>();

        Assert.Equal("application/json; charset=utf-8", GetHeaderValue(response, "Content-Type"));
        Assert.NotNull(result);
        Assert.Equal(1, result!.Count);
    }

    [Fact]
    public async Task Create_Forum_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData(new ForumDTO() { Title = "Title", Description = "Description" });
        HttpResponseData response = await _controller.CreateForum(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _forumService.Verify(s => s.CreateForum(It.IsAny<Forum>()), Times.Once);
    }

    [Fact]
    public async Task Create_Forum_Should_Throw_Null_Reference_Exception()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();

        await Assert.ThrowsAsync<NullReferenceException>(() => _controller.CreateForum(request));
        _forumService.Verify(s => s.CreateForum(It.IsAny<Forum>()), Times.Never);
    }

    [Fact]
    public async Task Get_Forum_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetForum(request, "1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Upate_Forum_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData(new UpdateForumDTO() { Title = "Title 2" });
        HttpResponseData response = await _controller.UpdateForum(request, "1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _forumService.Verify(s => s.UpdateForum("1", It.IsAny<UpdateForumDTO>()), Times.Once);
    }

    [Fact]
    public async Task Update_Forum_Should_Throw_Not_Found_Exception()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData(new UpdateForumDTO() { Title = "Title 2" });

        await Assert.ThrowsAsync<NotFoundException>(() => _controller.UpdateForum(request, "INVALID"));
    }

    [Fact]
    public async Task Create_Forum_Reply_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData(new ForumReplyDTO() { Text = "Text" });
        HttpResponseData response = await _controller.CreateForumReply(request, "1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_Forum_Reply_Should_Throw_Null_Reference_Exception()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();

        await Assert.ThrowsAsync<NullReferenceException>(() => _controller.CreateForumReply(request, "1"));
    }

    [Fact]
    public async Task Create_Forum_Reply_Should_Throw_Not_Found_Exception()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData(new ForumReplyDTO() { Text = "Text" });

        await Assert.ThrowsAsync<NotFoundException>(() => _controller.CreateForumReply(request, "INVALID"));
    }

    [Fact]
    public async Task Update_Forum_Reply_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData(new UpdateForumReplyDTO() { Text = "Text 2" });
        HttpResponseData response = await _controller.UpdateForumReply(request, "1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _forumService.Verify(s => s.UpdateForumReply("1", It.IsAny<UpdateForumReplyDTO>()), Times.Once);
    }

    [Fact]
    public async Task Update_Forum_Reply_Should_Throw_Not_Found_Exception()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData(new UpdateForumReplyDTO() { Text = "Text 2" });

        await Assert.ThrowsAsync<NotFoundException>(() => _controller.UpdateForumReply(request, "INVALID"));
    }

    [Fact]
    public async Task Delete_Forumn_Reply_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.DeleteForumReply(request, "1", "1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        _forumService.Verify(s => s.DeleteForumReply("1", "1"), Times.Once);
    }

    [Fact]
    public async Task Delete_Forum_Reply_Should_Throw_Not_Found_Exception()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();

        await Assert.ThrowsAsync<NotFoundException>(() => _controller.DeleteForumReply(request, "INVALID", "1"));
        await Assert.ThrowsAsync<NotFoundException>(() => _controller.DeleteForumReply(request, "1", "INVALID"));
    }
}

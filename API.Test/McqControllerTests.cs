using System.Net;
using API.Controllers;
using API.Test.Mock;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Model;
using Model.Response;
using Moq;
using Service.Interfaces;
using Xunit;

namespace API.Test;
public class McqControllerTests : ControllerTestsBase
{
    private readonly Mock<IMcqService> _mcqService;
    private readonly McqController _controller;

    static List<Mcq> mcqs = new() { new Mcq() { AllowRandom = true, Id = "1", AnswerOptions = new List<AnswerOption>() } };

    public McqControllerTests()
    {
        _mcqService = new Mock<IMcqService>();
        _controller = new(new LoggerFactory(), _tokenService.Object, _mcqService.Object, _mapper);

        _mcqService.Setup(x => x.GetAllMcqsAsync(int.MaxValue, 0)).ReturnsAsync(mcqs);
        _mcqService.Setup(x => x.GetMcqByIdAsync(mcqs[0].Id)).ReturnsAsync(mcqs[0]);
    }

    [Fact]
    public async Task Get_Mcqs_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetAllMcqs(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Mcqs_Async_should_return_A_Json()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetAllMcqs(request);
        ICollection<McqResponse>? result = await response.ReadFromJsonAsync<McqResponse[]>();

        Assert.Equal("application/json; charset=utf-8", GetHeaderValue(response, "Content-Type"));
        Assert.NotNull(result);
        Assert.Equal(1, result!.Count);
    }

    [Fact]
    public async Task Get_Mcq_By_Id_Should_Return_A_Mcq()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetMcqById(request, "1");
        McqResponse? result = await response.ReadFromJsonAsync<McqResponse>();

        Assert.NotNull(result);
        Assert.Equal(mcqs[0].Id, result?.Id);

    }

    [Fact]
    public async Task Get_Mcq_By_Id_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetMcqById(request, "1");

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
}

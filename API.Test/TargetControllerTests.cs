using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using API.Controllers;
using API.Test.Mock;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Model.Response;
using Model;
using Moq;
using Service.Interfaces;
using Xunit;

namespace API.Test;
public class TargetControllerTests : ControllerTestsBase
{
    private readonly Mock<ITargetService> _targetService;
    private readonly TargetController _controller;

    static List<Target> targets = new() { new Target() { Id ="1", ImageUrl= "", Description ="this is a description", Label= "best target", TargetExplanation="", Mcqs = new List<Mcq>(), Sets = new List<Set>() } };
    public TargetControllerTests()
    {
        _targetService = new();
        _controller = new(new LoggerFactory(), _tokenService.Object, _targetService.Object, _mapper);

        _targetService.Setup(x => x.GetAllTargetsAsync()).ReturnsAsync(targets);
        _targetService.Setup(x => x.GetTargetByIdAsync(targets[0].Id)).ReturnsAsync(targets[0]);
    }

    [Fact]
    public async Task Get_Targets_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetAllTargets(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Targets_Async_should_return_A_Json()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetAllTargets(request);
        ICollection<TargetResponse>? result = await response.ReadFromJsonAsync<TargetResponse[]>();

        Assert.Equal("application/json; charset=utf-8", GetHeaderValue(response, "Content-Type"));
        Assert.NotNull(result);
        Assert.Equal(1, result!.Count);
    }

    [Fact]
    public async Task Get_Target_By_Id_Should_Return_A_Target()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetTargetById(request, "1");
        TargetResponse? result = await response.ReadFromJsonAsync<TargetResponse>();

        Assert.NotNull(result);
        Assert.Equal(targets[0].Id, result?.Id);

    }

    [Fact]
    public async Task Get_Target_By_Id_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetTargetById(request, "1");

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
}

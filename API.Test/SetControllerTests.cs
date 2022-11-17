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
public class SetControllerTests : ControllerTestsBase
{
    private readonly Mock<ISetService> _setService;
    private readonly SetController _controller;

    static List<Set> sets = new() { new Set() { Id = "1", Users = new List<User>(), Targets = new List<Target>() } };

    public SetControllerTests()
    {
        _setService = new Mock<ISetService>();
        _controller = new(new LoggerFactory(), _tokenService.Object, _setService.Object, _mapper);

        _setService.Setup(x => x.GetAllSetsAsync(int.MaxValue, 0)).ReturnsAsync(sets);
        _setService.Setup(x => x.GetSetByIdAsync(sets[0].Id)).ReturnsAsync(sets[0]);
    }

    [Fact]
    public async Task Get_Sets_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetAllSets(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_All_Sets_Async_should_return_A_Json()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetAllSets(request);
        ICollection<SetResponse>? result = await response.ReadFromJsonAsync<SetResponse[]>();

        Assert.Equal("application/json; charset=utf-8", GetHeaderValue(response, "Content-Type"));
        Assert.NotNull(result);
        Assert.Equal(1, result!.Count);
    }

    [Fact]
    public async Task Get_Set_By_Id_Should_Return_A_Set()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetSetById(request, "1");
        Group? result = await response.ReadFromJsonAsync<Group>();

        Assert.NotNull(result);
        Assert.Equal(sets[0].Id, result?.Id);

    }

    [Fact]
    public async Task Get_Group_By_Id_Should_Respond_OK()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();
        HttpResponseData response = await _controller.GetSetById(request, "1");

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using API.Controllers;
using API.Test.Mock;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Model.Response;
using Moq;
using Service;
using Service.Exceptions;
using Service.Interfaces;
using Xunit;

namespace API.Test;
public class LoginControllerTests : ControllerTestsBase
{
    private readonly LoginController _controller;

    public LoginControllerTests()
    {
        _controller = new(new LoggerFactory(), _tokenService.Object);
    }

    [Fact]
    public async Task Authenticate_Should_Respond_Ok()
    {
        HttpRequestData request = MockHelpers.CreateHttpRequestData();

        HttpResponseData response = await _controller.Authenticate(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Authenticate_Should_Respond_Json()
    {
        LoginDTO loginDTO = new() { Email = "MarySue@gmail.com", Password = "M4rySu3san#22!" };

        HttpRequestData request = MockHelpers.CreateHttpRequestData(loginDTO);
        HttpResponseData response = await _controller.Authenticate(request);
        ICollection<LoginResponse>? result = await response.ReadFromJsonAsync<LoginResponse[]>();

        Assert.Equal("application/json; charset=utf-8", GetHeaderValue(response, "Content-Type"));
    }

    [Fact]
    public async Task Authenticate_Should_Throw_Null_Reference_Exception()
    {
        HttpRequestData request = null;

        await Assert.ThrowsAsync<NullReferenceException>(() => _controller.Authenticate(request));
    }
}
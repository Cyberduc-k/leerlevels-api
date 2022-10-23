using System.Net;
using API.Attributes;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model.DTO;
using Model.Response;
using Newtonsoft.Json;
using Service.Interfaces;

namespace API.Controllers;
public class LoginController : ControllerBase
{
    private readonly ITokenService TokenService;

    public LoginController(ILoggerFactory loggerFactory, ITokenService tokenService)
        : base(loggerFactory.CreateLogger<LoginController>())
    {
        TokenService = tokenService;
    }

    [Function(nameof(Authenticate))]
    [OpenApiOperation(operationId: "Login", tags: new[] { "Login" }, Summary = "Login for a user", Description = "This method logs in the user, and retrieves a JWT bearer token.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoginDTO), Required = true, Description = "The user credentials")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoginResponse), Description = "Login success")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "A login error has occured.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound)]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError)]
    public async Task<HttpResponseData> Authenticate([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")] HttpRequestData req, FunctionContext executionContext)
    {
        _logger.LogInformation("C# HTTP trigger function processed the Login request.");

        LoginDTO login = JsonConvert.DeserializeObject<LoginDTO>(await new StreamReader(req.Body).ReadToEndAsync())!;

        LoginResponse result = await TokenService.Login(login);

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result);

        return response;
    }
}
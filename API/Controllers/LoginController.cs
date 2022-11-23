using System.Net;
using API.Attributes;
using API.Examples;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model.DTO;
using Model.Response;
using Newtonsoft.Json;
using Service;
using Service.Interfaces;
using Service.Exceptions;

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
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoginDTO), Required = true, Description = "The user credentials", Example = typeof(LoginExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoginResponse), Description = "Login success", Example = typeof(LoginResponseExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "A login error has occured.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "An error occured while attempting to login", Example = typeof(ErrorResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError)]
    public async Task<HttpResponseData> Authenticate([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the Login request.");

        LoginDTO login = JsonConvert.DeserializeObject<LoginDTO>(await new StreamReader(req.Body).ReadToEndAsync())!;

        LoginResponse result = await TokenService.Login(login);

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result);

        return response;
    }

    [Function(nameof(Refresh))]
    [OpenApiOperation(operationId: "Refresh", tags: new[] { "Login"}, Summary = "Get a refresh token", Description = "provides a new refresh token when given a non-expired refresh token")]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(RefreshResponse), Description = "Login success", Example = typeof(RefreshResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "A valid, non-expired refresh token is required to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the refresh token user")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> Refresh([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "refresh")] HttpRequestData req)
    {
        // note: I think I want to move this endpoint to a new controller entirely so I can then also use our controller with authentication base class (would probably be better)
        if (!await TokenService.AuthenticationValidation(req)) {
            throw new AuthenticationException("the provided refresh token is invalid");
        }

        _logger.LogInformation("C# HTTP trigger function processed the Refresh request.");

        RefreshResponse result = await TokenService.Refresh(req);

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result);

        return response;
    }
}
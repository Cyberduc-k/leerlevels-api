using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Service.Interfaces;
using Model.DTO;
using Model.Response;
using Model;
using System.Security.Claims;

namespace Controller.Controllers;
public class Login
{
    private readonly ILogger _logger;
    private readonly ITokenService TokenService;

    public Login(ILoggerFactory loggerFactory, ITokenService tokenService)
    {
        _logger = loggerFactory.CreateLogger<Login>();
        TokenService = tokenService;
    }

    [Function(nameof(Authenticate))]
    [OpenApiOperation(operationId: "Login", tags: new[] { "Login" }, Summary = "Login for a user", Description = "This method logs in the user, and retrieves a JWT bearer token.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(LoginDTO), Required = true, Description = "The user credentials")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(LoginResponse), Description = "Login success")]
    public async Task<HttpResponseData> Authenticate([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")] HttpRequestData req, FunctionContext executionContext)
    {
        LoginDTO login = JsonConvert.DeserializeObject<LoginDTO>(await new StreamReader(req.Body).ReadToEndAsync())!; //if null then new by default 2do: exception handling (?? new LoginDTO() but better)

        LoginResponse result = await TokenService.CreateToken(login);

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result);

        return response;
    }

    /*[Function("GetUsers")]
    [ExampleAuth]
    [OpenApiOperation(operationId: "GetUsers", tags: new[] { "Users" }, Summary = "Get the users in the application", Description = "Some description.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(User[]), Description = "The users in the system")]
    public HttpResponseData GetUsers([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req, FunctionContext executionContext)
    {
        ClaimsPrincipal user = executionContext.GetUser();

        if (user == null) {
            HttpResponseData unauthortized = req.CreateResponse(HttpStatusCode.Unauthorized);
            return unauthortized;
        }


        string Name = user.Identity.Name;


        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

        response.WriteString("Welcome to Azure Functions!");

        return response;
    }*/

}

/*public class ExampleAuthAttribute : OpenApiSecurityAttribute
{
    public ExampleAuthAttribute() : base("ExampleAuth", SecuritySchemeType.Http)
    {
        Description = "JWT for authorization";
        In = OpenApiSecurityLocationType.Header;
        Scheme = OpenApiSecuritySchemeType.Bearer;
        BearerFormat = "JWT";
    }
}*/

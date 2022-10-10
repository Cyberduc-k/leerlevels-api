using System.Net;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Model.Response;
using Service.Interfaces;

namespace Controller.Controllers;

public class UsersController
{
    private readonly ILogger _logger;
    private readonly IUserService _UserService;

    public UsersController(ILoggerFactory loggerFactory, IUserService userservice)
    {
        _logger = loggerFactory.CreateLogger<UsersController>();
        _UserService = userservice;
    }

    [Function(nameof(GetUsers))]
    [OpenApiOperation(operationId: nameof(GetUsers), tags: new[] { "Users" }, Summary = "A list of users.", Description = "Will return a (full) list of users if a teacher or admin token is used.")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse[]), Description = "A list of users.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve users.")]
    public async Task<HttpResponseData> GetUsers([HttpTrigger(AuthorizationLevel.Anonymous /*2do: fix authorization*/, "get", Route = "users")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the GetUsers request.");

        ICollection<User> users = await _UserService.GetUsersAsync();

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(users);

        return res;
    }

    [Function(nameof(GetUserById))]
    [OpenApiOperation(operationId: nameof(GetUserById),tags: new[] { "Users" }, Summary = "A single user.", Description = "Will return a specified user's info for a logged in user or from the full list of users if a teacher, coach or administrator token is used")]
    [OpenApiParameter(name: "Id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The user id parameter.")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse), Description = "A single retrieved user.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve users.")]
    public async Task<HttpResponseData> GetUserById([HttpTrigger(AuthorizationLevel.Anonymous /*2do: fix authorization*/, "get", Route = "user/{id}")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the GetUser request.");

        string userId = req.Query("Id");
        User user = await _UserService.GetUserByIdAsync(userId);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(user);

        return res;
    }
}

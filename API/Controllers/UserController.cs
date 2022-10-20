using System.Net;
using API.Attributes;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Model.DTO;
using Model.Response;
using Newtonsoft.Json;
using Service;
using Service.Interfaces;
using API.Examples;

namespace API.Controllers;

public class UserController
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(ILoggerFactory loggerFactory, IMapper mapper, IUserService userservice)
    {
        _logger = loggerFactory.CreateLogger<UserController>();
        _mapper = mapper;
        _userService = userservice;
    }

    // Get users

    [Function(nameof(GetUsers))]
    [OpenApiOperation(operationId: nameof(GetUsers), tags: new[] { "Users" }, Summary = "A list of users", Description = "Will return a (full) list of users if a teacher or admin token is used.")]
    //[OpenApiSecurity("LeerLevelsAuthentication", SecuritySchemeType.Http, In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer, BearerFormat = "JWT")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse[]), Description = "A list of users.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve users.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound)]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError)]
    public async Task<HttpResponseData> GetUsers([HttpTrigger(AuthorizationLevel.Anonymous /*2do: fix authorization*/, "get", Route = "users")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the GetUsers request.");

        ICollection<User> users = await _userService.GetUsers();
        IEnumerable<UserResponse> userResponses = users.Select(u => _mapper.Map<UserResponse>(u));
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(userResponses);

        return res;
    }

    // Get user

    [Function(nameof(GetUserById))]
    [OpenApiOperation(operationId: nameof(GetUserById),tags: new[] { "Users" }, Summary = "A single user", Description = "Will return a specified user's info for a logged in user or from the full list of users if a teacher, coach or administrator token is used")]
    [OpenApiParameter(name: "Id", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The user id parameter.")]
    // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse), Description = "A single retrieved user.", Example = typeof(UserResponseExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve users.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound)]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError)]
    public async Task<HttpResponseData> GetUserById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/{id}")] HttpRequestData req,
        string userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed the GetUser request.");

        //string userId = req.Query("Id"); delete this line
        User user = await _userService.GetUserById(userId);

        // map retrieved user to the UserRepsonse model
        UserResponse userResponse = _mapper.Map<UserResponse>(user);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(userResponse);

        return res;
    }

    // Post user

    [Function(nameof(CreateUser))]
    [OpenApiOperation(operationId: nameof(CreateUser), tags: new[] { "Users" }, Summary = "Create a new user", Description = "Will create and return the new user.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserDTO), Required = true, Description = "Data for the user that has to be created.")]
    // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse), Description = "The newly created user.", Example = typeof(UserResponseExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to create a new user.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound)]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError)]
    public async Task<HttpResponseData> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the CreateUser request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(body)!;
        User user = _mapper.Map<User>(userDTO);
        User newUser = await _userService.CreateUser(user);
        UserResponse userResponse = _mapper.Map<UserResponse>(newUser);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(userResponse);

        return res;
    }

    // Update user

    [Function(nameof(UpdateUser))]
    [OpenApiOperation(operationId: nameof(UpdateUser), tags: new[] { "Users" }, Summary = "Edit a user", Description = "Allows for modification of a user.")]
    [OpenApiParameter(name: "Id", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The user id parameter.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateUserDTO), Required = true, Description = "The edited user data.", Example = typeof(UpdateUserExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse), Description = "The updated user", Example = typeof(UserResponseExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to update the user.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound)]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError)]
    public async Task<HttpResponseData> UpdateUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route= "users/{id}")] HttpRequestData req,
        string userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed the UpdateUser request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UpdateUserDTO updateUserDTO = JsonConvert.DeserializeObject<UpdateUserDTO>(body)!;

        await _userService.UpdateUser(userId, updateUserDTO);

        return req.CreateResponse(HttpStatusCode.OK);

    }

    // Delete user (soft)

    [Function(nameof(DeleteUser))]
    [OpenApiOperation(operationId: nameof(DeleteUser), tags: new[] { "Users" }, Summary = "Delete a user", Description = "Allows for the soft-deletion of a user.")]
    [OpenApiParameter(name: "Id", In = ParameterLocation.Path, Type = typeof(Guid), Required = true, Description = "The user id parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The user has been soft deleted (no longer active).")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound)]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError)]
    public async Task<HttpResponseData> DeleteUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "users/{id}")] HttpRequestData req,
        string userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed the DeleteUser request.");

        await _userService.DeleteUser(userId);

        return req.CreateResponse(HttpStatusCode.OK);
    }
}

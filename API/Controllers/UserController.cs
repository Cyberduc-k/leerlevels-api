using System.Net;
using API.Attributes;
using API.Examples;
using API.Exceptions;
using API.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Model.DTO;
using Model.Response;
using Newtonsoft.Json;
using Service.Interfaces;

namespace API.Controllers;

public class UserController : ControllerWithAuthentication
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(ILoggerFactory loggerFactory, ITokenService tokenService, IMapper mapper, IUserService userservice)
        : base(loggerFactory.CreateLogger<UserController>(), tokenService)
    {
        _mapper = mapper;
        _userService = userservice;
    }

    [Function(nameof(GetUsers))]
    [OpenApiOperation(operationId: nameof(GetUsers), tags: new[] { "Users" }, Summary = "A list of users", Description = "Will return a list of users if a teacher or admin token is used.")]
    [OpenApiAuthentication]
    [OpenApiParameter("limit", In = ParameterLocation.Query, Type = typeof(int), Required = false)]
    [OpenApiParameter("page", In = ParameterLocation.Query, Type = typeof(int), Required = false)]
    [OpenApiParameter("filter", In = ParameterLocation.Query, Type = typeof(string), Required = false)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Paginated<UserResponse>), Description = "A list of users.", Example = typeof(UserResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find a list of users.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetUsers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/users");
        _logger.LogInformation("C# HTTP trigger function processed the GetUsers request.");

        int limit = req.Query("limit").GetInt(int.MaxValue) ?? throw new InvalidQueryParameterException("limit");
        int page = req.Query("page").GetInt() ?? throw new InvalidQueryParameterException("page");
        string? filter = req.Query("filter").FirstOrDefault();
        ICollection<User> users = filter is null
            ? await _userService.GetUsers(limit, page)
            : await _userService.GetUsersFiltered(limit, page, filter);
        UserResponse[] userResponses = _mapper.Map<UserResponse[]>(users);
        Paginated<UserResponse> paginated = new(userResponses, page);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(paginated);
        return res;
    }

    [Function(nameof(GetUserById))]
    [OpenApiOperation(operationId: nameof(GetUserById), tags: new[] { "Users" }, Summary = "A single user", Description = "Will return a specified user's info for a logged in user or from the full list of users if a teacher, coach or administrator token is used")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The user id parameter.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse), Description = "A single retrieved user.", Example = typeof(UserResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the user.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetUserById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userId}")] HttpRequestData req,
        string userId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/users/{userId}");
        _logger.LogInformation("C# HTTP trigger function processed the GetUser request.");

        User user = await _userService.GetUserById(userId);
        UserResponse userResponse = _mapper.Map<UserResponse>(user);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(userResponse);

        return res;
    }

    // Get current user
    [Function(nameof(GetCurrentUser))]
    [OpenApiOperation(operationId: nameof(GetCurrentUser), tags: new[] { "Users" }, Summary = "The current user", Description = "Will return the current user's info for a logged in user")]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse), Description = "The current etrieved user.", Example = typeof(UserResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the user.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetCurrentUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/user")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/users/user");

        _logger.LogInformation("C# HTTP trigger function processed the GetCurrentUser request.");

        string userId = _tokenService.GetTokenClaim(req, "userId");

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
    [OpenApiAuthentication]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserDTO), Required = true, Description = "Data for the user that has to be created.", Example = typeof(UserExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse), Description = "The newly created user.", Example = typeof(UserResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error occured while trying to create the user.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/users");

        _logger.LogInformation("C# HTTP trigger function processed the CreateUser request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(body)!;
        User user = _mapper.Map<User>(userDTO);
        user.Password = _tokenService.EncryptPassword(user, user.Password);
        User newUser = await _userService.CreateUser(user);
        UserResponse userResponse = _mapper.Map<UserResponse>(newUser);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(userResponse);

        return res;
    }

    // Update user

    [Function(nameof(UpdateUser))]
    [OpenApiOperation(operationId: nameof(UpdateUser), tags: new[] { "Users" }, Summary = "Edit a user", Description = "Allows for modification of a user.")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The user id parameter.")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateUserDTO), Required = true, Description = "The edited user data.", Example = typeof(UpdateUserExample))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserResponse), Description = "The updated user", Example = typeof(UserResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error occured while trying to update the user.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the user to update.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> UpdateUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "users/{userId}")] HttpRequestData req,
        string userId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/users/{userId}");

        _logger.LogInformation("C# HTTP trigger function processed the UpdateUser request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UpdateUserDTO updateUserDTO = JsonConvert.DeserializeObject<UpdateUserDTO>(body)!;

        //retrieve the user first (have to use it to encrypt the password now)
        User user = await _userService.GetUserById(userId);

        // encrypt the password if one has been entered, otherwise, there is nu password given for the user to update and just don't update it
        if (updateUserDTO.Password != null) {
            updateUserDTO.Password = _tokenService.EncryptPassword(user, updateUserDTO.Password);
        }

        await _userService.UpdateUser(userId, updateUserDTO);

        return req.CreateResponse(HttpStatusCode.OK);

    }

    // Delete user (soft)

    [Function(nameof(DeleteUser))]
    [OpenApiOperation(operationId: nameof(DeleteUser), tags: new[] { "Users" }, Summary = "Delete a user", Description = "Allows for the soft-deletion of a user.")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The user id parameter.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The user has been soft deleted (no longer active).")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the user.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> DeleteUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "users/{userId}")] HttpRequestData req,
        string userId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/users/{userId}");

        //if user is not your token user
        if (userId != _tokenService.GetTokenClaim(req, "userId")!) {
            _logger.LogInformation("C# HTTP trigger function processed the DeleteUser request.");

            await _userService.DeleteUser(userId);

            return req.CreateResponse(HttpStatusCode.OK);
        } else {
            _logger.LogInformation("attempt to delete own user detected!");
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
    }

    // Get user groups

    [Function(nameof(GetUserGroups))]
    [OpenApiOperation(operationId: nameof(GetUserGroups), tags: new[] { "Users" }, Summary = "A list of a user's groups", Description = "Returs a list of groups the user is a member of.")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The user id parameter.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GroupResponse[]), Description = "A list of groups the user is in.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find a list of a user's groups.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetUserGroups([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userId}/groups")] HttpRequestData req,
        string userId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/users/{userId}/groups");

        _logger.LogInformation("C# HTTP trigger function processed the GetUserGroups request.");

        ICollection<Group> groups = await _userService.GetUserGroups(userId);
        GroupResponse[] groupResponses = _mapper.Map<GroupResponse[]>(groups);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(groupResponses);

        return res;
    }

    //Get user sets

    [Function(nameof(GetUserSets))]
    [OpenApiOperation(operationId: nameof(GetUserSets), tags: new[] { "Users" }, Summary = "A list of a user's sets", Description = "Returns a list of sets the user has created or started working on.")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The user id parameter.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SetResponse[]), Description = "A list of sets the user is in.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find a list of a user's sets.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetUserSets([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userId}/sets")] HttpRequestData req,
        string userId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/users/{userId}/sets");

        _logger.LogInformation("C# HTTP trigger function processed the GetUserSets request.");

        ICollection<Set> sets = await _userService.GetUserSets(userId);
        SetResponse[] setResponses = _mapper.Map<SetResponse[]>(sets);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(setResponses);

        return res;
    }
}

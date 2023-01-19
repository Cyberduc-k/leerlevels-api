using System.Net;
using API.Attributes;
using API.Examples;
using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Model.DTO;
using Model.Response;
using Newtonsoft.Json;
using Service.Interfaces;
using Group = Model.Group;

namespace API.Controllers;
public class GroupController : ControllerWithAuthentication
{
    private readonly IGroupService _groupService;
    private readonly IMapper _mapper;

    public GroupController(ILoggerFactory loggerFactory, ITokenService tokenService, IGroupService groupService, IMapper mapper)
        : base(loggerFactory.CreateLogger<GroupController>(), tokenService)
    {
        _groupService = groupService;
        _mapper = mapper;
    }

    [Function(nameof(GetAllGroups))]
    [OpenApiOperation(operationId: "getGroups", tags: new[] { "Groups" })]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GroupResponse[]), Description = "The OK response", Example = typeof(GroupResponseExample[]))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve groups.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetAllGroups([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "groups")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/groups");

        _logger.LogInformation("C# HTTP trigger function processed the getGroups request.");

        ICollection<Group> groups = await _groupService.GetAllGroupsAsync();
        GroupResponse[] groupResponses = _mapper.Map<GroupResponse[]>(groups);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(groupResponses);

        return res;
    }

    [Function(nameof(GetGroupById))]
    [OpenApiOperation(operationId: "getGroup", tags: new[] { "Groups" })]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "groupId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The group ID parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GroupResponse), Description = "The OK response", Example = typeof(GroupResponseExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve the group.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the group with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetGroupById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "groups/{groupId}")] HttpRequestData req,
        string groupId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/groups/{groupId}");

        Group group = await _groupService.GetGroupByIdAsync(groupId);
        GroupResponse groupResponse = _mapper.Map<GroupResponse>(group);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(groupResponse);

        return res;
    }

    [Function(nameof(AddUserToGroup))]
    [OpenApiOperation(operationId: nameof(AddUserToGroup), tags: new[] { "Groups" }, Summary = "Add user to a group", Description = "Add the current user to the group.")]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "groupId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The group ID parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(AddGroupToUserResponse), Description = "The newly created Group.", Example = typeof(GroupResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error occured while trying to create the group.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> AddUserToGroup([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "groups/users/{groupId}")] HttpRequestData req, string groupId)
    {
        var loggeduser = await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "groups/users/{groupId}");

        _logger.LogInformation("C# HTTP trigger function processed the Create Group request.");

        Group group = await _groupService.AddGrouptoUser(groupId, loggeduser);
        AddGroupToUserResponse groupResponse = _mapper.Map<AddGroupToUserResponse>(group);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.Created);

        await res.WriteAsJsonAsync(groupResponse);

        return res;
    }

    [Function(nameof(CreateGroup))]
    [OpenApiOperation(operationId: nameof(CreateGroup), tags: new[] { "Groups" }, Summary = "Create a new group", Description = "Creates a new group")]
    [OpenApiAuthentication]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GroupDTO), Required = true, Description = "The new group")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GroupResponse), Description = "The group is created", Example = typeof(GroupResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> CreateGroup(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "groups")] HttpRequestData req
        )
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Administrator, "/groups");
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        GroupDTO groupDTO = JsonConvert.DeserializeObject<GroupDTO>(body)!;
        Group group = _mapper.Map<Group>(groupDTO);
        Group newGroup = await _groupService.CreateGroup(group);
        GroupResponse groupResponse = _mapper.Map<GroupResponse>(newGroup);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(groupResponse);

        return res;
    }

    [Function(nameof(UpdateGroup))]
    [OpenApiOperation(operationId: nameof(UpdateGroup), tags: new[] { "Groups" }, Summary = "Edit a group", Description = "Edits a group")]
    [OpenApiAuthentication]
    [OpenApiParameter("groupId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The group Id")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateGroupDTO), Required = true, Description = "The edited group")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The group is edited")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the group with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> UpdateGroup(
        [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "groups/{groupId}")] HttpRequestData req,
        string groupId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Administrator, "/groups/{groupId}");
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UpdateGroupDTO groupDTO = JsonConvert.DeserializeObject<UpdateGroupDTO>(body)!;

        await _groupService.UpdateGroup(groupId, groupDTO);

        return req.CreateResponse(HttpStatusCode.OK);
    }

    [Function(nameof(DeleteGroup))]
    [OpenApiOperation(operationId: nameof(DeleteGroup), tags: new[] { "Groups" }, Summary = "Delete a group", Description = "Deletes a group")]
    [OpenApiAuthentication]
    [OpenApiParameter("groupId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The group Id")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The group is removed")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the group with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> DeleteGroup(
        [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "groups/{groupId}")] HttpRequestData req,
        string groupId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Administrator, "/groups/{groupId}");
        await _groupService.DeleteGroup(groupId);

        return req.CreateResponse(HttpStatusCode.OK);
    }
}

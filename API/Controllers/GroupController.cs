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
using Model.Response;
using Service.Interfaces;
using Group = Model.Group;
using Newtonsoft.Json;


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

    [Function(nameof(CreateGroup))]
    [OpenApiOperation(operationId: nameof(CreateGroup), tags: new[] { "Groups" }, Summary = "Create a new Group", Description = "Will create and return the new Group.")]
    [OpenApiAuthentication]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(CreateGroupDTO), Required = true, Description = "Data for the Group that has to be created.", Example = typeof(Group))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(CreateGroupResponse), Description = "The newly created Group.", Example = typeof(GroupResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.BadRequest, Description = "An error occured while trying to create the group.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> CreateGroup([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "groups")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Administrator, "/groups");

        _logger.LogInformation("C# HTTP trigger function processed the Create Group request.");

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        CreateGroupDTO groupDTO = JsonConvert.DeserializeObject<CreateGroupDTO>(body)!;
        Group group = _mapper.Map<Group>(groupDTO);
        Group newGroup = await _groupService.CreateGroup(group);
        CreateGroupResponse groupResponse = _mapper.Map<CreateGroupResponse>(newGroup);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(groupResponse);

        return res;
    }
}

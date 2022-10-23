using System.Net;
using API.Attributes;
using API.Validation;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Service.Interfaces;
using Group = Model.Group;

namespace API.Controllers;
public class GroupController : ControllerWithAuthentication
{
    private readonly IGroupService _groupService;
    private readonly GetGroupByIdValidator validationRules;

    public GroupController(ILoggerFactory loggerFactory, ITokenService tokenService, IGroupService groupService, GetGroupByIdValidator validations)
        : base(loggerFactory.CreateLogger<GroupController>(), tokenService)
    {
        _groupService = groupService;
        validationRules = validations;
    }

    // Get groups

    [Function(nameof(GetAllGroups))]
    [OpenApiOperation(operationId: "getGroups", tags: new[] { "Groups" })]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Group>), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve groups.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find a list of groups.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetAllGroups([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "groups")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Administrator, "/groups");

        _logger.LogInformation("C# HTTP trigger function processed the getGroups request.");

        ICollection<Group> groups = await _groupService.GetAllGroupsAsync();

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(groups);

        return res;
    }

    // Get group

    [Function(nameof(GetGroupById))]
    [OpenApiOperation(operationId: "getGroup", tags: new[] { "Groups" })]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "groupId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The group ID parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Group), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a vlaid Group Id.")]
    public async Task<HttpResponseData> GetGroupById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "groups/{groupId}")] HttpRequestData req,
        string groupId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/groups/{groupId}");

        _logger.LogInformation("C# HTTP trigger function processed the getGroup request.");

        Group groupToValidate = new() { Id = groupId };
        ValidationResult result = await validationRules.ValidateAsync(groupToValidate);

        if (result.IsValid) {
            Group group = await _groupService.GetGroupByIdAsync(groupId);

            HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

            await res.WriteAsJsonAsync(group);

            return res;
        }
        return req.CreateResponse(HttpStatusCode.BadRequest);
    }
}

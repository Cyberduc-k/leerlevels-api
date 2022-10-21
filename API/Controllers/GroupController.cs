using System.Net;
using API.Validation;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Service.Interfaces;
using Group = Model.Group;

namespace API.Controllers;
public class GroupController
{
    private readonly ILogger _logger;
    private readonly IGroupService _groupService;
    private readonly GetGroupByIdValidator validationRules;

    public GroupController(ILoggerFactory loggerFactory, IGroupService groupservice, GetGroupByIdValidator validations)
    {
        _logger = loggerFactory.CreateLogger<GroupController>();
        _groupService = groupservice;
        validationRules = validations;
    }

    [Function(nameof(GetAllGroups))]
    [OpenApiOperation(operationId: "getGroups", tags: new[] { "Groups" })]
    // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Group>), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve groups.")]

    public async Task<HttpResponseData> GetAllGroups([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "groups")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the getGroups request.");

        ICollection<Group> groups = await _groupService.GetAllGroupsAsync();

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(groups);

        return res;
    }

    [Function(nameof(GetGroupById))]
    [OpenApiOperation(operationId: "getGroup", tags: new[] { "Groups" })]
    [OpenApiParameter(name: "groupId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The group ID parameter")]
    // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Group), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a vlaid Group Id.")]

    public async Task<HttpResponseData> GetGroupById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "groups/{id}")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the getGroup request.");

        string groupId = req.Query("groupId");

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

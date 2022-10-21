using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Newtonsoft.Json;
using Service.Interfaces;
using System.Text.RegularExpressions;
using Group = Model.Group;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using API.Validation;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

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

        ICollection<Model.Group> groups = await _groupService.GetAllGroupsAsync();

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

        var groupToValidate = new Group { Id = groupId };
        var result = this.validationRules.Validate(groupToValidate);    

        if (result.IsValid) {
            Group group = await _groupService.GetGroupByIdAsync(groupId);

            HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

            await res.WriteAsJsonAsync(group);

            return res;
        }
        return req.CreateResponse(HttpStatusCode.NotFound);
    }
}

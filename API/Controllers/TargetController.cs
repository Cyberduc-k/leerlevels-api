using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Service.Interfaces;

namespace API.Controllers;
public class TargetController
{
    private readonly ILogger _logger;
    private readonly ITargetService _targetService;

    public TargetController(ILoggerFactory loggerFactory, ITargetService targetservice)
    {
        _logger = loggerFactory.CreateLogger<TargetController>();
        _targetService = targetservice;
    }

    [Function(nameof(GetAllTargets))]
    [OpenApiOperation(operationId: "getTargets", tags: new[] { "Targets" })]
    // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Target>), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve targets.")]

    public async Task<HttpResponseData> GetAllTargets([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "targets")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the GetUsers request.");

        ICollection<Target> targets = await _targetService.GetAllTargetsAsync();

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(targets);

        return res;
    }

    [Function(nameof(GetTargetById))]
    [OpenApiOperation(operationId: "getTarget", tags: new[] { "Targets" })]
    [OpenApiParameter(name: "targetId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The target ID parameter")]
    // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Mcq), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a vlaid Target Id.")]

    public async Task<HttpResponseData> GetTargetById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "targets/{id}")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string targetId = req.Query("targetId");
        Target target = await _targetService.GetTargetByIdAsync(targetId);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(target);

        return res;
    }
}

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
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Service.Interfaces;
using Model;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;

namespace API.Controllers;
public class SetController
{
    private readonly ILogger _logger;
    private readonly ISetService _setService;

    public SetController(ILoggerFactory loggerFactory, ISetService setservice)
    {
        _logger = loggerFactory.CreateLogger<SetController>();
        _setService = setservice;
    }

    [Function(nameof(GetAllSets))]
    [OpenApiOperation(operationId: "getsets", tags: new[] { "Sets" })]
    // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Set>), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve sets.")]

    public async Task<HttpResponseData> GetAllSets([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "sets")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the getsets request.");

        ICollection<Set> sets = await _setService.GetAllSetsAsync();

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(sets);

        return res;
    }


    [Function(nameof(GetSetById))]
    [OpenApiOperation(operationId: "getSet", tags: new[] { "Sets" })]
    [OpenApiParameter(name: "setId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The set ID parameter")]
    // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Set), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a vlaid Set Id.")]

    public async Task<HttpResponseData> GetSetById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "sets/{id}")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the getSet request.");

        string setId = req.Query("setId");
        Set set = await _setService.GetSetByIdAsync(setId);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(set);

        return res;
    }
}

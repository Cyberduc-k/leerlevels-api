using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Service.Interfaces;
using Model;
using Service;

namespace API.Controllers;
public class McqController
{
    private readonly ILogger _logger;
    private readonly IMcqService mcqService; 

    public McqController(ILogger logger, IMcqService mcqservice)
    {
        _logger = logger; 
        mcqService = mcqservice ;
    }

    [Function(nameof(GetAllMcqs))]
    [OpenApiOperation(operationId: "getMcqs", tags: new[] { "Mcqs" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(List<Mcq>), Description = "The OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "text/plain", bodyType: typeof(string), Description = "Bad Request")]

    public HttpResponseData GetAllMcqs([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "mcqs")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

        var res =  mcqService.GetAllAsync();

        return response;
    }


    [Function(nameof(GetMcqById))]
    [OpenApiOperation(operationId: "getMcq", tags: new[] { "Mcqs" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "mcqId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The mcq ID parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Mcq), Description = "The OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "text/plain", bodyType: typeof(string), Description = "Bad Request")]

    public HttpResponseData GetMcqById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "mcqs/mcqId{id}")] HttpRequestData req, string mcqId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);

        var res = mcqService.GetByIdAsync(mcqId);

        return response;
    }


}

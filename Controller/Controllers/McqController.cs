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
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;

namespace API.Controllers;
public class McqController
{
    private readonly ILogger _logger;
    private readonly IMcqService _mcqService; 

    public McqController(ILoggerFactory loggerFactory, IMcqService mcqservice)
    {
        _logger = loggerFactory.CreateLogger<McqController>();
        _mcqService = mcqservice ;
    }

    [Function(nameof(GetAllMcqs))]
    [OpenApiOperation(operationId: "getMcqs", tags: new[] { "Mcqs" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(List<Mcq>), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve mcqs.")]

    public async Task<HttpResponseData> GetAllMcqs([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "mcqs")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the GetUsers request.");

        ICollection<Mcq> mcqs = await _mcqService.GetAllMcqsAsync();

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(mcqs);

        return res;
    }


    [Function(nameof(GetMcqById))]
    [OpenApiOperation(operationId: "getMcq", tags: new[] { "Mcqs" })]
    [OpenApiParameter(name: "mcqId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The mcq ID parameter")]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Mcq), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a vlaid Mcq Id.")]

    public async Task<HttpResponseData> GetMcqById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "mcqs/{id}")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        string mcqId = req.Query("mcqId");
        Mcq mcq = await _mcqService.GetMcqByIdAsync(mcqId);

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(mcq);

        return res;
    }
}

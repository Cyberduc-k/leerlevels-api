using System.Net;
using System.Text.RegularExpressions;
using API.Validators;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Model.Response;
using Service.Interfaces;

namespace API.Controllers;
public class McqController
{
    private readonly ILogger _logger;
    private readonly IMcqService _mcqService;
    private readonly IMapper _mapper;
    private readonly GetMcqByIdValidator validationRules;

    public McqController(ILoggerFactory loggerFactory, IMcqService mcqservice, IMapper mapper, GetMcqByIdValidator validations)
    {
        _logger = loggerFactory.CreateLogger<McqController>();
        _mcqService = mcqservice;
        _mapper = mapper;
        validationRules = validations;  
    }

    [Function(nameof(GetAllMcqs))]
    [OpenApiOperation(operationId: "getMcqs", tags: new[] { "Mcqs" })]
    //  [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<McqResponse>), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve mcqs.")]

    public async Task<HttpResponseData> GetAllMcqs([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "mcqs")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the getMcqs request.");

        ICollection<Mcq> mcqs = await _mcqService.GetAllMcqsAsync();
        IEnumerable<McqResponse> mappedMcqs = mcqs.Select(f => _mapper.Map<McqResponse>(f));

        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(mappedMcqs);

        return res;
    }

    [Function(nameof(GetMcqById))]
    [OpenApiOperation(operationId: "getMcq", tags: new[] { "Mcqs" })]
    [OpenApiParameter(name: "mcqId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The mcq ID parameter")]
    // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Mcq), Description = "The OK response")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a vlaid Mcq Id.")]

    public async Task<HttpResponseData> GetMcqById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "mcqs/{id}")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed the getMcq request.");
        string mcqId = req.Query("mcqId");

        Mcq mcqToValidate = new() { Id = mcqId };
        ValidationResult result = await validationRules.ValidateAsync(mcqToValidate);

        if (result.IsValid) {
            Mcq mcq = await _mcqService.GetMcqByIdAsync(mcqId);
            Mcq mappedMcq = _mapper.Map<Mcq>(mcq);

            HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

            await res.WriteAsJsonAsync(mappedMcq);

            return res;
        }
        return req.CreateResponse(HttpStatusCode.BadRequest);
    }
}

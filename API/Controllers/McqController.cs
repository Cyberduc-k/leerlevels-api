using System.Net;
using API.Attributes;
using API.Examples;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Model;
using Model.Response;
using Service.Interfaces;

namespace API.Controllers;
public class McqController : ControllerWithAuthentication
{
    private readonly IMcqService _mcqService;
    private readonly IMapper _mapper;

    public McqController(ILoggerFactory loggerFactory, ITokenService tokenService, IMcqService mcqservice, IMapper mapper)
        : base(loggerFactory.CreateLogger<McqController>(), tokenService)
    {
        _mcqService = mcqservice;
        _mapper = mapper;
    }

    [Function(nameof(GetAllMcqs))]
    [OpenApiOperation(operationId: "getMcqs", tags: new[] { "Mcqs" })]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(McqResponse[]), Description = "The OK response", Example = typeof(McqResponseExample[]))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve mcqs.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]

    public async Task<HttpResponseData> GetAllMcqs([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "mcqs")] HttpRequestData req)
    {
          await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/mcqs");

        _logger.LogInformation("C# HTTP trigger function processed the getMcqs request.");

        ICollection<Mcq> mcqs = await _mcqService.GetAllMcqsAsync();
        IEnumerable<McqResponse> mappedMcqs = mcqs.Select(f => _mapper.Map<McqResponse>(f));
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(mappedMcqs);

        return res;
    }

    [Function(nameof(GetMcqById))]
    [OpenApiOperation(operationId: "getMcq", tags: new[] { "Mcqs" })]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "mcqId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The mcq ID parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(McqResponse), Description = "The OK response", Example = typeof(McqResponseExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a vlaid Mcq Id.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the Mcq with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]

    public async Task<HttpResponseData> GetMcqById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "mcqs/{mcqId}")] HttpRequestData req,
        string mcqId)
    {
           await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/mcqs/{mcqId}");

        _logger.LogInformation("C# HTTP trigger function processed the getMcq request.");

        Mcq mcq = await _mcqService.GetMcqByIdAsync(mcqId);
        McqResponse mappedMcq = _mapper.Map<McqResponse>(mcq);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(mappedMcq);

        return res;
    }
}

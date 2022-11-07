using System.Net;
using API.Attributes;
using API.Examples;
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
public class SetController : ControllerWithAuthentication
{
    private readonly ISetService _setService;
    private readonly IMapper _mapper;

    public SetController(ILoggerFactory loggerFactory, ITokenService tokenService, ISetService setservice, IMapper mapper)
        : base(loggerFactory.CreateLogger<SetController>(), tokenService)
    {
        _setService = setservice;
        _mapper = mapper;
    }

    // Get sets

    [Function(nameof(GetAllSets))]
    [OpenApiOperation(operationId: "getsets", tags: new[] { "Sets" })]
    [OpenApiAuthentication]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SetResponse[]), Description = "The OK response", Example = typeof(SetResponseExample[]))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve sets.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetAllSets([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "sets")] HttpRequestData req)
    {
          await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/sets");

        _logger.LogInformation("C# HTTP trigger function processed the getsets request.");

        ICollection<Set> sets = await _setService.GetAllSetsAsync();
        IEnumerable<SetResponse> setResponses = sets.Select(u => _mapper.Map<SetResponse>(u));


        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(setResponses);

        return res;
    }

    // Get set

    [Function(nameof(GetSetById))]
    [OpenApiOperation(operationId: "getSet", tags: new[] { "Sets" })]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "setId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The set ID parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SetResponse), Description = "The OK response", Example = typeof(SetResponseExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a vlaid Set Id.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the Set with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetSetById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "sets/{setId}")] HttpRequestData req,
        string setId)
    {
         await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/sets/{setId}");

        _logger.LogInformation("C# HTTP trigger function processed the getSet request.");


        Set set = await _setService.GetSetByIdAsync(setId);
        SetResponse setResponses = _mapper.Map<SetResponse>(set);


        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(set);

        return res;

    }
}

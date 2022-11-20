using System.Net;
using API.Attributes;
using API.Examples;
using API.Exceptions;
using API.Extensions;
using AutoMapper;
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
public class TargetController : ControllerWithAuthentication
{
    private readonly IMapper _mapper;
    private readonly ITargetService _targetService;

    public TargetController(
        ILoggerFactory loggerFactory,
        ITokenService tokenService,
        ITargetService targetservice,
        IMapper mapper)
        : base(loggerFactory.CreateLogger<TargetController>(), tokenService)
    {
        _targetService = targetservice;
        _mapper = mapper;
    }

    [Function(nameof(GetAllTargets))]
    [OpenApiOperation(operationId: "getTargets", tags: new[] { "Targets" })]
    [OpenApiAuthentication]
    [OpenApiParameter("limit", In = ParameterLocation.Query, Type = typeof(int), Required = false)]
    [OpenApiParameter("page", In = ParameterLocation.Query, Type = typeof(int), Required = false)]
    [OpenApiParameter("filter", In = ParameterLocation.Query, Type = typeof(string), Required = false)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Paginated<TargetResponse>), Description = "The OK response", Example = typeof(TargetResponseExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve targets.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetAllTargets([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "targets")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/targets");

        _logger.LogInformation("C# HTTP trigger function processed the GetUsers request.");

        int limit = req.Query("limit").GetInt(int.MaxValue) ?? throw new InvalidQueryParameterException("limit");
        int page = req.Query("page").GetInt() ?? throw new InvalidQueryParameterException("page");
        string? filter = req.Query("filter").FirstOrDefault();
        ICollection<Target> targets = filter is null
            ? await _targetService.GetAllTargetsAsync(limit, page)
            : await _targetService.GetAllTargetsFilteredAsync(limit, page, filter);
        TargetResponse[] targetResponses = targets.Select(t => _mapper.Map<TargetResponse>(t)).ToArray();
        Paginated<TargetResponse> paginated = new(targetResponses, page);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(paginated);
        return res;
    }

    [Function(nameof(GetTargetById))]
    [OpenApiOperation(operationId: "getTarget", tags: new[] { "Targets" })]
    [OpenApiAuthentication]
    [OpenApiParameter(name: "targetId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The target ID parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(TargetResponse), Description = "The OK response", Example = typeof(TargetResponseExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "Please enter a vlaid Target Id.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the Target with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetTargetById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "targets/{targetId}")] HttpRequestData req,
        string targetId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/targets/{targetId}");

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        Target target = await _targetService.GetTargetByIdAsync(targetId);
        TargetResponse targetResponse = await _mapper.Map<Task<TargetResponse>>(target);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(targetResponse);
        return res;
    }
}

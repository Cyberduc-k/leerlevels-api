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

    [Function(nameof(GetAllSets))]
    [OpenApiOperation(nameof(GetAllSets), tags: "Sets")]
    [OpenApiAuthentication]
    [OpenApiParameter("limit", In = ParameterLocation.Query, Type = typeof(int), Required = false)]
    [OpenApiParameter("page", In = ParameterLocation.Query, Type = typeof(int), Required = false)]
    [OpenApiParameter("filter", In = ParameterLocation.Query, Type = typeof(string), Required = false)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SetResponse[]), Description = "The OK response", Example = typeof(SetResponseExample[]))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve sets.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetAllSets([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "sets")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/sets");

        _logger.LogInformation("C# HTTP trigger function processed the getsets request.");

        int limit = req.Query("limit").GetInt(int.MaxValue) ?? throw new InvalidQueryParameterException("limit");
        int page = req.Query("page").GetInt() ?? throw new InvalidQueryParameterException("page");
        string? filter = req.Query("filter").FirstOrDefault();
        ICollection<Set> sets = filter is null
            ? await _setService.GetAllSetsAsync(limit, page)
            : await _setService.GetAllSetsFilteredAsync(limit, page, filter);
        SetResponse[] setResponses = _mapper.Map<SetResponse[]>(sets);
        Paginated<SetResponse> paginated = new(setResponses, page);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(paginated);

        return res;
    }

    [Function(nameof(GetSetById))]
    [OpenApiOperation(nameof(GetSetById), tags: "Sets")]
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
        SetResponse setResponse = _mapper.Map<SetResponse>(set);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(setResponse);

        return res;
    }
}

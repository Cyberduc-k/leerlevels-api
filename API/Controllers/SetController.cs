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
using Model.DTO;
using Model.Response;
using Newtonsoft.Json;
using Service;
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
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Paginated<SetResponse>), Description = "The OK response", Example = typeof(SetResponseExample))]
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

    [Function(nameof(CreateSet))]
    [OpenApiOperation(operationId: nameof(CreateSet), tags: new[] { "Sets" }, Summary = "Create a new set", Description = "Creates a new set")]
    [OpenApiAuthentication]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(SetDTO), Required = true, Description = "The new set")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(SetResponse), Description = "The set is created", Example = typeof(SetResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> CreateSet(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "sets")] HttpRequestData req
        )
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Administrator, "/sets");
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        SetDTO setDTO = JsonConvert.DeserializeObject<SetDTO>(body)!;
        Set set = _mapper.Map<Set>(setDTO);
        Set newSet = await _setService.CreateSet(set);
        SetResponse setResponse = _mapper.Map<SetResponse>(newSet);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(setResponse);

        return res;
    }

    [Function(nameof(UpdateSet))]
    [OpenApiOperation(operationId: nameof(UpdateSet), tags: new[] { "Sets" }, Summary = "Edit a set", Description = "Edits a set")]
    [OpenApiAuthentication]
    [OpenApiParameter("setId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The set Id")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateSetDTO), Required = true, Description = "The edited set")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The set is edited")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the set with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> UpdateSet(
        [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "sets/{setId}")] HttpRequestData req,
        string setId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Administrator, "/sets/{setId}");
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UpdateSetDTO setDTO = JsonConvert.DeserializeObject<UpdateSetDTO>(body)!;

        await _setService.UpdateSet(setId, setDTO);

        return req.CreateResponse(HttpStatusCode.OK);
    }

    [Function(nameof(DeleteSet))]
    [OpenApiOperation(operationId: nameof(DeleteSet), tags: new[] { "Sets" }, Summary = "Delete a set", Description = "Deletes a set")]
    [OpenApiAuthentication]
    [OpenApiParameter("setId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The set Id")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The set is removed")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the set with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> DeleteSet(
        [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "sets/{setId}")] HttpRequestData req,
        string setId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Administrator, "/sets/{setId}");
        await _setService.DeleteSet(setId);

        return req.CreateResponse(HttpStatusCode.OK);
    }
}

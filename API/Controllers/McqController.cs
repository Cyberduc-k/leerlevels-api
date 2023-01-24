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
    [OpenApiOperation(nameof(GetAllMcqs), tags: "Mcqs")]
    [OpenApiAuthentication]
    [OpenApiParameter("limit", In = ParameterLocation.Query, Type = typeof(int), Required = false)]
    [OpenApiParameter("page", In = ParameterLocation.Query, Type = typeof(int), Required = false)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Paginated<McqResponse>), Description = "The OK response", Example = typeof(McqResponseExample))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Description = "An error has occured while trying to retrieve mcqs.")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> GetAllMcqs([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "mcqs")] HttpRequestData req)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/mcqs");

        _logger.LogInformation("C# HTTP trigger function processed the getMcqs request.");

        int limit = req.Query("limit").GetInt(int.MaxValue) ?? throw new InvalidQueryParameterException("limit");
        int page = req.Query("page").GetInt() ?? throw new InvalidQueryParameterException("page");
        ICollection<Mcq> mcqs = await _mcqService.GetAllMcqsAsync(limit, page);
        McqResponse[] mappedMcqs = _mapper.Map<McqResponse[]>(mcqs);
        Paginated<McqResponse> paginated = new(mappedMcqs, page);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(paginated);

        return res;
    }

    [Function(nameof(GetMcqById))]
    [OpenApiOperation(nameof(GetMcqById), tags: "Mcqs")]
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
        string userId = await ValidateAuthenticationAndAuthorization(req, UserRole.Student, "/mcqs/{mcqId}");
        Mcq mcq = await _mcqService.GetMcqByIdAsync(mcqId);
        McqResponse mappedMcq = await _mapper.Map<Task<McqResponse>>((mcq, userId));
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(mappedMcq);

        return res;
    }

    [Function(nameof(CreateMcq))]
    [OpenApiOperation(operationId: nameof(CreateMcq), tags: new[] { "Mcqs" }, Summary = "Create a new mcq", Description = "Creates a new mcq")]
    [OpenApiAuthentication]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(McqDTO), Required = true, Description = "The new mcq")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(McqResponse), Description = "The mcq is created", Example = typeof(McqResponseExample))]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> CreateMcq(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "mcqs")] HttpRequestData req
        )
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Administrator, "/mcqs");
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        McqDTO mcqDTO = JsonConvert.DeserializeObject<McqDTO>(body)!;
        Mcq mcq = _mapper.Map<Mcq>(mcqDTO);
        Mcq newMcq = await _mcqService.CreateMcq(mcq);
        McqResponse mcqResponse = _mapper.Map<McqResponse>(newMcq);
        HttpResponseData res = req.CreateResponse(HttpStatusCode.OK);

        await res.WriteAsJsonAsync(mcqResponse);

        return res;
    }

    [Function(nameof(UpdateMcq))]
    [OpenApiOperation(operationId: nameof(UpdateMcq), tags: new[] { "Mcqs" }, Summary = "Edit a mcq", Description = "Edits a mcq")]
    [OpenApiAuthentication]
    [OpenApiParameter("mcqId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The mcq Id")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UpdateMcqDTO), Required = true, Description = "The edited mcq")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The mcq is edited")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the mcq with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> UpdateMcq(
        [HttpTrigger(AuthorizationLevel.Anonymous, "PUT", Route = "mcqs/{mcqId}")] HttpRequestData req,
        string mcqId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Administrator, "/mcqs/{mcqId}");
        string body = await new StreamReader(req.Body).ReadToEndAsync();
        UpdateMcqDTO mcqDTO = JsonConvert.DeserializeObject<UpdateMcqDTO>(body)!;

        await _mcqService.UpdateMcq(mcqId, mcqDTO);

        return req.CreateResponse(HttpStatusCode.OK);
    }

    [Function(nameof(DeleteMcq))]
    [OpenApiOperation(operationId: nameof(DeleteMcq), tags: new[] { "Mcqs" }, Summary = "Delete a mcq", Description = "Deletes a mcq")]
    [OpenApiAuthentication]
    [OpenApiParameter("mcqId", In = ParameterLocation.Path, Type = typeof(string), Required = true, Description = "The mcq Id")]
    [OpenApiErrorResponse(HttpStatusCode.Unauthorized, Description = "Unauthorized to access this operation.")]
    [OpenApiErrorResponse(HttpStatusCode.Forbidden, Description = "Forbidden from performing this operation.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "The mcq is removed")]
    [OpenApiErrorResponse(HttpStatusCode.NotFound, Description = "Could not find the mcq with the specified Id.")]
    [OpenApiErrorResponse(HttpStatusCode.InternalServerError, Description = "An internal server error occured.")]
    public async Task<HttpResponseData> DeleteMcq(
        [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "mcqs/{mcqId}")] HttpRequestData req,
        string mcqId)
    {
        await ValidateAuthenticationAndAuthorization(req, UserRole.Administrator, "/mcqs/{mcqId}");
        await _mcqService.DeleteMcq(mcqId);

        return req.CreateResponse(HttpStatusCode.OK);
    }
}

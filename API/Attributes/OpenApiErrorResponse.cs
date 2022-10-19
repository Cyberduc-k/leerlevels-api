using System.Net;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Model.Response;

namespace API.Attributes;

public class OpenApiErrorResponse : OpenApiResponseWithBodyAttribute
{
    public OpenApiErrorResponse(HttpStatusCode statusCode)
        : base(statusCode, "application/json", typeof(ErrorResponse))
    {
    }
}

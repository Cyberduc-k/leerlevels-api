using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.Response;

public class ErrorResponse
{
    [JsonRequired]
    [OpenApiProperty(Description = "The message of an error", Nullable = false)]
    public string Message { get; set; }

    public ErrorResponse()
    {
    }

    public ErrorResponse(string message)
    {
        Message = message;
    }
}

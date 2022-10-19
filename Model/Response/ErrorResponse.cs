using Newtonsoft.Json;

namespace Model.Response;

public class ErrorResponse
{
    [JsonRequired]
    public string Message { get; set; }

    public ErrorResponse()
    {
    }

    public ErrorResponse(string message)
    {
        Message = message;
    }
}

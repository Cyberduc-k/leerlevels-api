using Newtonsoft.Json;

namespace Model.Response;

public class ErrorResponse
{
    [JsonRequired]
    public string Message { get; set; }
    public string? Source { get; set; }
    public string? StackTrace { get; set; }

    public ErrorResponse()
    {
    }

    public ErrorResponse(string message)
    {
        Message = message;
    }

    public ErrorResponse(Exception ex)
    {
        Message = ex.Message;
        Source = ex.Source;
        StackTrace = ex.StackTrace;
    }
}

using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace API.Test.Mock;

public static class HttpResponseDataExtensions
{
    public static async Task<string> ReadStringAsync(this HttpResponseData resp)
    {
        if (resp.Body is MemoryStream stream) {
            if (stream.Position != 0) stream.Position = 0;

            using StreamReader reader = new(stream);

            return await reader.ReadToEndAsync();
        }

        return string.Empty;
    }

    public static async Task<T?> ReadFromJsonAsync<T>(this HttpResponseData resp)
    {
        string json = await resp.ReadStringAsync();

        return JsonConvert.DeserializeObject<T>(json);
    }
}

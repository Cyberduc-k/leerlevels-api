using System.Text;
using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace API.Test.Mocks;

// taken from: https://github.com/lohithgn/az-fx-isolated-unittest/blob/main/test/Microsoft.Azure.Functions.Isolated.TestDoubles/MockHelpers.cs
public static class MockHelpers
{
    public static HttpRequestData CreateHttpRequestData<T>(T payload, string? token = null, string method = "GET")
    {
        string json = JsonConvert.SerializeObject(payload);

        return CreateHttpRequestData(json, token, method);
    }

    public static HttpRequestData CreateHttpRequestData(string? payload = null, string? token = null, string method = "GET")
    {
        string input = payload ?? string.Empty;
        FunctionContext functionContext = CreateContext(new NewtonsoftJsonObjectSerializer());
        MockHttpRequestData request = new(functionContext, method: method, body: new MemoryStream(Encoding.UTF8.GetBytes(input)));

        request.Headers.Add("Content-Type", "application/json");

        if (token is not null) request.Headers.Add("Authorization", $"Bearer {token}");

        return request;
    }

    public static FunctionContext CreateContext(ObjectSerializer? serializer = null)
    {
        MockFunctionContext context = new();
        ServiceCollection services = new();

        services.AddOptions();
        services.AddFunctionsWorkerCore(c => {
            //c.UseMiddleware();
            c.Serializer = serializer;
        });

        context.InstanceServices = services.BuildServiceProvider();

        return context;
    }
}

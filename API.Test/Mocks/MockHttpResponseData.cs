using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace API.Test.Mocks;

// taken from: https://github.com/lohithgn/az-fx-isolated-unittest/blob/main/test/Microsoft.Azure.Functions.Isolated.TestDoubles/MockHttpResponseData.cs
public class MockHttpResponseData : HttpResponseData
{
    public MockHttpResponseData(FunctionContext functionContext, HttpStatusCode status) : base(functionContext)
    {
        StatusCode = status;
    }

    public override HttpStatusCode StatusCode { get; set; }
    public override HttpHeadersCollection Headers { get; set; } = new HttpHeadersCollection();
    public override Stream Body { get; set; } = new MemoryStream();
    public override HttpCookies Cookies { get; }
}

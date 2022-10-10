using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace API.Middleware;

public class ExceptionMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try {
            await next(context);
        } catch (Exception ex) {
            if (await context.GetHttpRequestDataAsync() is HttpRequestData req) {
                HttpResponseData res = req.CreateResponse(HttpStatusCode.InternalServerError);

                await res.WriteAsJsonAsync(new { ex.Message }, res.StatusCode);

                InvocationResult invocation = context.GetInvocationResult();
                OutputBindingData<HttpResponseData>? binding = context.GetOutputBindings<HttpResponseData>().FirstOrDefault(b => b.BindingType == "http" && b.Name != "$return");

                if (binding is not null) {
                    binding.Value = res;
                } else {
                    invocation.Value = res;
                }
            }
        }
    }
}

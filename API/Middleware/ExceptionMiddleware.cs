using System.Net;
using API.Exceptions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Model.Response;
using Service.Exceptions;

namespace API.Middleware;

public class ExceptionMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger _logger;
    private readonly Dictionary<Type, HttpStatusCode> _statusCodes = new();

    public ExceptionMiddleware(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();

        AddHandler<NotImplementedException>(HttpStatusCode.NotImplemented);
        AddHandler<NotFoundException>(HttpStatusCode.NotFound);
        AddHandler<AuthenticationException>(HttpStatusCode.Unauthorized);
        AddHandler<AuthorizationException>(HttpStatusCode.Forbidden);
        AddHandler<InvalidQueryParameterException>(HttpStatusCode.BadRequest);
    }

    internal void AddHandler<TException>(HttpStatusCode statusCode) where TException : Exception
    {
        _statusCodes.Add(typeof(TException), statusCode);
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try {
            await next(context);
        } catch (Exception ex) {
            if (await context.GetHttpRequestDataAsync() is HttpRequestData req) {
                HttpResponseData res = req.CreateResponse(HttpStatusCode.InternalServerError);

                if (ex is AggregateException ae) {
                    ex = ae.InnerException!;
                }

                if (_statusCodes.TryGetValue(ex.GetType(), out HttpStatusCode code)) {
                    await res.WriteAsJsonAsync(new ErrorResponse(ex.Message), code);
                } else {
                    _logger.LogError(ex, ex.Message);
                }

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

using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Interfaces;
using Service.Exceptions;

namespace API.Middleware;

public class JwtMiddleware : IFunctionsWorkerMiddleware
{
    ITokenService TokenService { get; }
    ILogger Logger { get; }

    public JwtMiddleware(ITokenService tokenService, ILogger<JwtMiddleware> logger)
    {
        this.TokenService = tokenService;
        this.Logger = logger;
    }

    public async Task Invoke(FunctionContext Context, FunctionExecutionDelegate Next)
    {
        KeyValuePair<string, BindingMetadata> binding = Context.FunctionDefinition.InputBindings.FirstOrDefault(b => b.Value.Type == "httpTrigger");

        // only validate and check authentication when in an http trigger
        if (binding.Key != null) {
            string HeadersString = (string)Context.BindingContext.BindingData["Headers"]!;

            Dictionary<string, string> Headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(HeadersString)!;

            if (Headers.TryGetValue("Authorization", out string? AuthorizationHeader)) {
                try {
                    AuthenticationHeaderValue BearerHeader = AuthenticationHeaderValue.Parse(AuthorizationHeader);

                    //validation of token based on the validation parameters
                    ClaimsPrincipal Token = await TokenService.GetByValue(BearerHeader.Parameter!);
                        
                    Context.Items["userToken"] = Token;

                } catch (Exception e) {
                    Logger.LogError(e.Message);
                }
            }
        }

        await Next(Context);
    }
}

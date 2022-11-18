using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;
public class ErrorResponseExample : OpenApiExample<ErrorResponse>
{
    public override IOpenApiExample<ErrorResponse> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("UserNotFound", new ErrorResponse() { Message = "The user could not be found" }));

        return this;
    }
}

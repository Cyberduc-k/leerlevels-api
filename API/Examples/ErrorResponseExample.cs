using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;
public class ErrorResponseExample : OpenApiExample<ErrorResponse>
{
    public override IOpenApiExample<ErrorResponse> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("null reference error", new ErrorResponse() { Message = "Object reference not set to an instance of an object", Source = "Service", StackTrace = "Service.UserService.<CreateUser>d__35.MoveNext() in S:\\LeerLevels_repo\\LeerLevels\\Service\\UserService.cs:line 41" }));

        return this;
    }
}

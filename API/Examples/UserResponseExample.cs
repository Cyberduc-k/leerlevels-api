using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.OpenApi.Models;
using Model;
using Newtonsoft.Json.Serialization;

namespace API.Examples;
public class UserResponseExample : OpenApiExample<UserResponseOpenApiExample>
{
    public override IOpenApiExample<UserResponseOpenApiExample> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("Jan", new UserResponseOpenApiExample() { Id = "a75e3fe7-f519-48de-a106-79f788a1b479", Email = "jan@gmail.com", FirstName = "Jan", LastName = "Groothuis", UserName = "", Role = UserRole.Student, ShareCode = "DTRY-WQER-PIGU-VNSA" }, namingStrategy));

        return this;
    }
}

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.OpenApi.Models;
using Model;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;
public class UserResponseExample : OpenApiExample<UserResponse>
{
    public override IOpenApiExample<UserResponse> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("Jan", new UserResponse() { Id = "a75e3fe7-f519-48de-a106-79f788a1b479", Email = "jan@gmail.com", FirstName = "Jan", LastName = "Groothuis", UserName = "JanG#1", Role = UserRole.Student, ShareCode = "DTRY-WQER-PIGU-VNSA" }, namingStrategy));

        return this;
    }
}

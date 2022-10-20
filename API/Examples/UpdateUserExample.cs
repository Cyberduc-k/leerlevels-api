using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class UpdateUserExample : OpenApiExample<UpdateUserOpenApiExample>
{
    public override IOpenApiExample<UpdateUserOpenApiExample> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("John", new UpdateUserOpenApiExample() { Email = "JohnDoe@gmail.com", FirstName = "John", LastName = "Doe", UserName = "JohnD#1" }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Mary", new UpdateUserOpenApiExample() { Email = "MarySue@gmail.com", FirstName = "Mary", LastName = "Sue", UserName = "MarySue#22" }, namingStrategy));

        return this;
    }
}

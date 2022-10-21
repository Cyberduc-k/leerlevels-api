using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class UpdateUserExample : OpenApiExample<UpdateUserDTO>
{
    public override IOpenApiExample<UpdateUserDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("John", new UpdateUserDTO() { Email = "JohnDoe@gmail.com", FirstName = "John", LastName = "Doe", UserName = "JohnD#1" }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Mary", new UpdateUserDTO() { Email = "MarySue@gmail.com", FirstName = "Mary", LastName = "Sue", UserName = "MarySue#22" }, namingStrategy));

        return this;
    }
}

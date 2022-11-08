using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;
public class LoginExample : OpenApiExample<LoginDTO>
{
    public override IOpenApiExample<LoginDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("Login Student John", new LoginDTO() { Email = "JohnDoe@gmail.com", Password = "J0hn#001!" }));
        Examples.Add(OpenApiExampleResolver.Resolve("Login Teacher Mary", new LoginDTO() { Email = "MarySue@gmail.com", Password = "M4rySu3san#22!" }));

        return this;
    }
}
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;
public class LoginExample : OpenApiExample<LoginDTO>
{
    public override IOpenApiExample<LoginDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("Login John", new LoginDTO() { Email = "JohnDoe@gmail.com", Password = "J0hnnyb0y#1!" }));

        return this;
    }
}
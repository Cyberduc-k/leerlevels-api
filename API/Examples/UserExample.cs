using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;
public class UserExample : OpenApiExample<UserDTO>
{
    public override IOpenApiExample<UserDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("John", new UserDTO() { Email = "John@gmail.com", FirstName = "John", LastName = "Doe", UserName = "JohnD#1", Password = "J0nh#001!", Role = UserRole.Student }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Mary", new UserDTO() { Email = "Mary@gmail.com", FirstName = "Mary", LastName = "Sue", UserName = "MarySue#22", Password = "M4rySu3san#22!", Role = UserRole.Teacher }, namingStrategy));

        return this;
    }
}
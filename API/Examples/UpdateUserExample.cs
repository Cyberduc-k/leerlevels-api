using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class UpdateUserExample : OpenApiExample<UpdateUserDTO>
{
    public override IOpenApiExample<UpdateUserDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("John", new UpdateUserDTO() { Email = "JohnDoe@gmail.com", FirstName = "John", LastName = "Doe", UserName = "JohnD#1", Password = "J0nh#001!", Role = UserRole.Student, IsActive = true }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Mary", new UpdateUserDTO() { Email = "MarySue@gmail.com", FirstName = "Mary", LastName = "Sue", UserName = "MarySue#22", Password = "M4rySu3san#22!", Role = UserRole.Teacher, IsActive = true }, namingStrategy));

        return this;
    }
}

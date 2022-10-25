using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model;
using Model.DTO;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Security.Cryptography;

namespace API.Examples;

public class UpdateUserExample : OpenApiExample<UpdateUserDTO>
{
    public override IOpenApiExample<UpdateUserDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("John", new UpdateUserDTO() { Email = "JohnDoe@gmail.com", FirstName = "John", LastName = "Doe", UserName = "JohnD#1", Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("J0nh#001!")), Role = UserRole.Student }, namingStrategy));
        Examples.Add(OpenApiExampleResolver.Resolve("Mary", new UpdateUserDTO() { Email = "MarySue@gmail.com", FirstName = "Mary", LastName = "Sue", UserName = "MarySue#22", Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("M4rySu3san#22!")), Role = UserRole.Student }, namingStrategy));

        return this;
    }
}

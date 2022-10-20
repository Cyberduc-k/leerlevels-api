using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Model;
using Newtonsoft.Json;

namespace API.Examples;

[OpenApiExample(typeof(UserResponseExample))]
public class UserResponseOpenApiExample
{
    [JsonRequired]
    [OpenApiProperty(Default = "12345678-1234-1234-1234-123456789101", Description = "The globally unique identifier of a user", Nullable = false)]
    public string Id { get; set; }

    [JsonRequired]
    [OpenApiProperty(Default = "email@mail.com", Description = "The email address of a user", Nullable = false)]
    public string Email { get; set; }

    [JsonRequired]
    [OpenApiProperty(Default = "John", Description = "The first name of a user", Nullable = false)]
    public string FirstName { get; set; }

    [JsonRequired]
    [OpenApiProperty(Default = "Doe", Description = "The last name of a user", Nullable = false)]
    public string LastName { get; set; }

    [JsonRequired]
    [OpenApiProperty(Default = "JohnnyD", Description = "The username for a user", Nullable = false)]
    public string UserName { get; set; }

    [JsonRequired]
    [OpenApiProperty(Default = UserRole.Student, Description = "The assigned role of a user", Nullable = false)]
    public UserRole Role { get; set; }

    [JsonRequired]
    [OpenApiProperty(Default = "AAAA-BBBB-CCCC-DDDD", Description ="The share code for the group/sets of a user", Nullable = false)]
    public string ShareCode { get; set; }
}

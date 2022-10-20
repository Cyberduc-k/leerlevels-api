using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace API.Examples;

[OpenApiExample(typeof(UpdateUserExample))]
public class UpdateUserOpenApiExample
{
    [OpenApiProperty(Default = "email@mail.com", Description = "The new email address for a user to be updated", Nullable = true)]
    public string? Email { get; set; }

    [OpenApiProperty(Default = "John", Description = "The new first name of a user", Nullable = true)]
    public string? FirstName { get; set; }

    [OpenApiProperty(Default = "Doe", Description = "The new last name of a user", Nullable = true)]
    public string? LastName { get; set; }

    [OpenApiProperty(Default = "JohnnyD", Description = "The new username for a user", Nullable = true)]
    public string? UserName { get; set; }
}

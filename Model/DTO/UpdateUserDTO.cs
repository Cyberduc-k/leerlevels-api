using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class UpdateUserDTO
{
    [OpenApiProperty(Default = "email@mail.com", Description = "The new email address for a user to be updated", Nullable = true)]
    public string? Email { get; set; }

    [OpenApiProperty(Default = "John", Description = "The new first name of a user", Nullable = true)]
    public string? FirstName { get; set; }

    [OpenApiProperty(Default = "Doe", Description = "The new last name of a user", Nullable = true)]
    public string? LastName { get; set; }

    [OpenApiProperty(Default = "JohnnyD", Description = "The new username of a user", Nullable = true)]
    public string? UserName { get; set; }

    [OpenApiProperty(Default = "J0nh#001!", Description = "The new password of a user", Nullable = true)]
    public string? Password { get; set; }

    [OpenApiProperty(Default = "Student", Description = "The new role of a user", Nullable = true)]
    public UserRole? Role { get; set; }

    [OpenApiProperty(Default = true, Description = "The active status of a user (false when deleted)", Nullable = true)]
    public bool? IsActive { get; set; }
}

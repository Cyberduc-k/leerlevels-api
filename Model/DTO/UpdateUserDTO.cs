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

    [OpenApiProperty(Default = UserRole.Student, Description = "The new role of a user", Nullable = true)]
    public UserRole? Role { get; set; }

    [OpenApiProperty(Default = true, Description = "The (changed) login status of a user", Nullable = true)]
    public bool? IsLoggedIn { get; set; }

    //if we want to get rid of the delete user function (which now only updateds the IsActive bool, we can just put that here and actually use the DeleteUser endpoint to REALLY delete the user)
    /*
    [OpenApiProperty(Default = "", Description = "", Nullable = true)]
    public IsActive? { get; set;} */
}

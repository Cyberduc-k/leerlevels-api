using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.DTO;

public class UserDTO
{
    [JsonRequired]
    [OpenApiProperty(Default = "electronic@mail.com", Description = "The email address of a user", Nullable = false)]
    public string Email { get; set; }

    [JsonRequired]
    [OpenApiProperty(Default = "Harry", Description = "The first name of a user", Nullable = false)]
    public string FirstName { get; set; }

    [JsonRequired]
    [OpenApiProperty(Default = "Davis", Description = "The last name of a user", Nullable = false)]
    public string LastName { get; set; }

    [JsonRequired]
    [OpenApiProperty(Default = "HarryDav", Description = "The username of a user", Nullable = false)]
    public string UserName { get; set; }

    [JsonRequired]
    [OpenApiProperty(Default = "HarryDave#123!", Description = "The password of a user", Nullable = false)]
    public string Password { get; set; }

    [JsonRequired]
    [OpenApiProperty(Default = UserRole.Student, Description = "The role of a user", Nullable = false)]
    public UserRole Role { get; set; }

    public UserDTO()
    {
    }

    public UserDTO(string email, string firstName, string lastName, string userName, string password, UserRole role)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Password = password;
        Role = role;
    }
}

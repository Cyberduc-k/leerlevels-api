using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.DTO;

public class LoginDTO
{
    [JsonRequired]
    [OpenApiProperty(Default = "johndoe@mail.com", Description = "A user's email address", Nullable = false)]
    public string Email { get; set; }

    [JsonRequired]
    [OpenApiProperty(Default = "M4rySu3san#22!", Description = "The password of a user", Nullable = false)]
    public string Password { get; set; }

    public LoginDTO()
    {
    }

    public LoginDTO(string email, string password)
    {
        Email = email;
        Password = password;
    }
}

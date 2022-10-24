using Newtonsoft.Json;

namespace Model.DTO;

public class LoginDTO
{
    [JsonRequired]
    public string Email { get; set; }

    [JsonRequired]
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

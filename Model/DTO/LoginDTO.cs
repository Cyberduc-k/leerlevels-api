using Newtonsoft.Json;

namespace Model.DTO;

public class LoginDTO
{
    [JsonRequired]
    public string UserName { get; set; }

    [JsonRequired]
    public string Password { get; set; }

    public LoginDTO()
    {
    }

    public LoginDTO(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}

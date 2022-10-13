using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


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

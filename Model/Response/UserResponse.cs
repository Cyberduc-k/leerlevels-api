using Newtonsoft.Json;

namespace Model.Response;
public class UserResponse
{
    [JsonRequired]
    public string Id { get; set; }

    [JsonRequired]
    public string Email { get; set; }

    [JsonRequired]
    public string FirstName { get; set; }

    [JsonRequired]
    public string LastName { get; set; }

    [JsonRequired]
    public string UserName { get; set; }

    [JsonRequired]
    public UserRole Role { get; set; }

    [JsonRequired]
    public string ShareCode { get; set; }

    public UserResponse()
    {
    }

    public UserResponse(string id, string email, string firstname, string lastname, string username, UserRole role, string sharecode)
    {
        Id = id;
        Email = email;
        FirstName = firstname;
        LastName = lastname;
        UserName = username;
        Role = role;
        ShareCode = sharecode;
    }
}

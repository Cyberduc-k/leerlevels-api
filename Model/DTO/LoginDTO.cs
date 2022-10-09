namespace Model.DTO;

public class LoginDTO
{
    public string UserName { get; set; }
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

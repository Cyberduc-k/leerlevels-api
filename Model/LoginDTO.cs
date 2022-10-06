namespace Model;
public class LoginDTO
{
    public string userName { get; set; }
    public string Password { get; set; }

    public LoginDTO()
    {

    }

    public LoginDTO(string userName, string password)
    {
        this.userName = userName;
        Password = password;
    }
}

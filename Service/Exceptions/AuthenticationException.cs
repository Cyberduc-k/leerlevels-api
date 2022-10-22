namespace Service.Exceptions;
public class AuthenticationException : Exception
{
    public AuthenticationException(string message): base($"Authentication error: {message}")
    {
    }
}

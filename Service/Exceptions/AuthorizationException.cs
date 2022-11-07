namespace Service.Exceptions;
public class AuthorizationException : Exception
{
    public AuthorizationException(string message) : base($"Authorization error: {message}")
    {
    }
}

namespace API.Exceptions;

public class InvalidQueryParameterException : Exception
{
    public InvalidQueryParameterException(string name) : base($"invalid query parameter {name}")
    {
    }
}

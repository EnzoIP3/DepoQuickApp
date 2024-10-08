namespace BusinessLogic.Auth.Exceptions;

public class AuthException : Exception
{
    public AuthException(string message)
        : base(message)
    {
    }
}

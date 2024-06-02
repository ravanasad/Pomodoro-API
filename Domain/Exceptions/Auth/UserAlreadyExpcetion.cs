using System.Net;

namespace Domain.Exceptions.Auth;

public class UserAlreadyExpcetion : BaseException
{
    public UserAlreadyExpcetion() : base("User already exists.", HttpStatusCode.BadRequest)
    {
    }
}

public class UserNotFoundException : BaseException
{
    public UserNotFoundException() : base("User not found.", HttpStatusCode.NotFound)
    {
    }
}

public class InvalidPasswordException : BaseException
{
    public InvalidPasswordException() : base("Invalid password.", HttpStatusCode.BadRequest)
    {
    }
}

public class RegisterExceptions : BaseException
{
    public RegisterExceptions(string errors) : base(errors, HttpStatusCode.BadRequest)
    {
    }
}
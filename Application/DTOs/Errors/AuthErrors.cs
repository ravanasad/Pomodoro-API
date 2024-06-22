
namespace Application.DTOs.Errors;
public static class AuthErrors
{
    public static ErrorDesc CredentialsError => new("Invalid Credentials", "The provided credentials are invalid. Please check your username and password and try again.");

    public static ErrorDesc UserAlreadyExists => new("User already exists", "A user with the provided email or username already exists. Please try again with a different email or username.");
}

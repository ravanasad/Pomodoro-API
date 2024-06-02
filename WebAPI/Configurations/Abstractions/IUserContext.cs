namespace WebAPI.Configurations.Abstractions;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
    string Username { get; }
}

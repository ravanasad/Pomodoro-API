namespace Application.Configurations;

public interface IUserContext
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
    string Username { get; }
}

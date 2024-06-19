namespace Application.Services.AuthService.Google;

public interface IGoogleAuthService
{
    Task AuthenticateAsync(GoogleAuthenticateDto request);
}

public sealed record GoogleAuthenticateDto(string Id, string IdToken, string Email, string Username, string PhotoUrl, string provider);
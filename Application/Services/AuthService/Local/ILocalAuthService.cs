using Application.DTOs;

namespace Application.Services.AuthService.Local;

public interface ILocalAuthService
{
    Task<Result> LoginAsync(LoginDto request);
    Task<Result> RegisterAsync(RegisterDto request);
}


using Domain.Entities.Auth;
using System.Security.Claims;

namespace Application.Services.TokenService;

public interface ITokenService
{
    Task<Result> GenerateToken(AppUser user, IList<string> roles);
    Result<string> GenerateRefreshToken();
    Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
    Task<Result<string>> GenerateTokenFromRefreshToken(string refreshToken);
}

using Domain.Entities.Auth;
using System.Security.Claims;

namespace Application.Services.TokenService;

public interface ITokenService
{
    Task GenerateToken(AppUser user, IList<string> roles);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    string GenerateTokenFromRefreshToken(string refreshToken);
}


using Application.Services.TokenService;

namespace Infrastructure.Services.TokenService;

public sealed class TokenService(IHttpContextAccessor contextAccessor,
    UserManager<AppUser> userManager,
    IOptions<TokenSetting> tokenSettings
    ) : ITokenService
{
    private HttpContext Context => contextAccessor.HttpContext!;
    private TokenSetting setting => tokenSettings.Value;
    public async Task GenerateToken(AppUser user, IList<string> roles)
    {
        var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
            };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(setting.Secret));

        var token = new JwtSecurityToken(
                issuer: setting.Issuer,
                audience: setting.Audience,
                expires: DateTime.Now.AddMinutes(setting.Expiration),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
        JwtSecurityTokenHandler tokenHandler = new();
        var option = new CookieOptions()
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddMinutes(setting.Expiration),
            SameSite = SameSiteMode.Strict,
            Secure = false
        };

        await userManager.AddClaimsAsync(user, claims);

        Context?.Response.Cookies.Append(setting.CokkieName, tokenHandler.WriteToken(token), option);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }


    public string GenerateTokenFromRefreshToken(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        throw new NotImplementedException();
    }
}


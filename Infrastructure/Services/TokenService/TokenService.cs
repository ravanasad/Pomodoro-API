using Application.Services.TokenService;

namespace Infrastructure.Services.TokenService
{
    public sealed class TokenService(IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager, IOptions<TokenSetting> tokenSettings) : ITokenService
    {
        private readonly HttpContext _context = contextAccessor.HttpContext!;
        private readonly UserManager<AppUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly TokenSetting _tokenSettings = tokenSettings?.Value ?? throw new ArgumentNullException(nameof(tokenSettings));


        public async Task<Result> GenerateToken(AppUser user, IList<string> roles)
        {
            if (user == null) return Result.Failure(Error.BadRequest(UserTaskErrors.UserTaskNotFound));
            if (roles == null) return Result.Failure(Error.BadRequest(TokenErrors.InvalidRole));

            var claims = GetClaims(user, roles);
            var token = CreateJwtToken(claims);
            var tokenHandler = new JwtSecurityTokenHandler();

            var addClaimsResult = await _userManager.AddClaimsAsync(user, claims);
            if (!addClaimsResult.Succeeded)
            {
                return Result.Failure(Error.InvalidRequest(TokenErrors.FailedToAddClaim));
            }

            AppendCookie(tokenHandler.WriteToken(token));

            return Result.Success();
        }

        public Result<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Result<string>.Success(Convert.ToBase64String(randomNumber));
        }

        public Result<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret)),
                    ValidateLifetime = false
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Result<ClaimsPrincipal>.Failure(Error.InvalidRequest(TokenErrors.InvalidToken));
                }

                return Result<ClaimsPrincipal>.Success(principal);
            }
            catch (Exception)
            {
                return Result<ClaimsPrincipal>.Failure(Error.InvalidRequest(TokenErrors.InvalidToken));
            }
        }

        public async Task<Result<string>> GenerateTokenFromRefreshToken(string refreshToken)
        {
            // Assuming a method exists to validate the refresh token and get the corresponding user
            var user = await ValidateRefreshToken(refreshToken);
            if (user == null)
            {
                return Result<string>.Failure(Error.InvalidRequest(TokenErrors.InvalidRefreshToken));
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await GenerateToken(user, roles);
            if (result.IsFailure)
            {
                return Result<string>.Failure(result.Error);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = CreateJwtToken(GetClaims(user, roles));
            return Result<string>.Success(tokenHandler.WriteToken(token));
        }

        private List<Claim> GetClaims(AppUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            return claims;
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
            return new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                expires: DateTime.Now.AddDays(_tokenSettings.Expiration),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
        }

        private void AppendCookie(string token)
        {
            var option = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddMinutes(_tokenSettings.Expiration),
                SameSite = SameSiteMode.Strict,
                Secure = false
            };

            _context?.Response.Cookies.Append(_tokenSettings.CookieName, token, option);
        }

        private async Task<AppUser?> ValidateRefreshToken(string refreshToken)
        {
            return await Task.FromResult<AppUser?>(null);
        }
    }
}

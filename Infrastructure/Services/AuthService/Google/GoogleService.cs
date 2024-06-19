using Application.Services.AuthService;
using Application.Services.TokenService;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.AuthService.Google;

public class GoogleService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public GoogleService(UserManager<AppUser> userManager, IConfiguration configuration, ITokenService tokenService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _tokenService = tokenService;
    }

    public async Task AuthenticateAsync(GoogleAuthenticateDto request)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [_configuration["Authentication:Google:ClientId"]]
        };

        var payload =await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

        var user = 
            await _userManager.FindByLoginAsync(request.provider, payload.Subject) ?? 
            await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            user = new AppUser
            {
                Email = request.Email,
                UserName = request.Username,
                PictureUrl = request.PhotoUrl
            };

            await _userManager.CreateAsync(user);
            await _userManager.AddLoginAsync(user, new(request.provider, payload.Subject, request.provider));
        }
        await _tokenService.GenerateToken(user, await _userManager.GetRolesAsync(user));
    }
}


using Application.DTOs;
using Application.DTOs.Common;
using Application.Services.AuthService.Local;
using Application.Services.TokenService;

namespace Infrastructure.Services.AuthService.Local;

public sealed class LocalAuthService(UserManager<AppUser> userManager,
                                     ITokenService tokenService) : ILocalAuthService
{
    private ITokenService TokenService { get; } = tokenService;

    public async Task<Result> LoginAsync(LoginDto request)
    {
        var user = await userManager.FindByEmailAsync(request.EmailOrUsername) ?? 
                   await userManager.FindByNameAsync(request.EmailOrUsername);

        if (user == null)
        {
            return Result.Failure("Invalid username or password.");
        }

        var result = await userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            return Result.Failure("Invalid username or password.");
        }

        var roles = await userManager.GetRolesAsync(user);
        await TokenService.GenerateToken(user, roles);
        return Result.Success();
    }

    public async Task<Result> RegisterAsync(RegisterDto request)
    {
        var user = await userManager.FindByEmailAsync(request.Email) ??
                   await userManager.FindByNameAsync(request.Username);

        if (user != null)
        {
            return Result.Failure("User already exists.");
        }

        IdentityResult result = await userManager.CreateAsync(new()
        {
            UserName = request.Username,
            Email = request.Email
        }, request.Password);

        if (!result.Succeeded)
        {
            return Result.Failure(result.Errors.Select(x => x.Description).ToList());
        }
        return Result.Success();
    }
}


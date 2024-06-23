using Application.Services.AuthService.Local;
using Application.Services.TokenService;

namespace Infrastructure.Services.AuthService.Local;

public sealed class LocalAuthService(UserManager<AppUser> userManager,
                                     ITokenService tokenService) : ILocalAuthService
{
    private ITokenService TokenService = tokenService;

    public async Task<Result> LoginAsync(LoginDto request)
    {
        var user = await userManager.FindByEmailAsync(request.EmailOrUsername) ?? 
                   await userManager.FindByNameAsync(request.EmailOrUsername);

        if (user == null)
        {
            return Result.Failure(Error.BadRequest(AuthErrors.CredentialsError));
        }

        var result = await userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            return Result.Failure(Error.BadRequest(AuthErrors.CredentialsError));
        }

        var roles = await userManager.GetRolesAsync(user);
        var tokenResult = await TokenService.GenerateToken(user, roles);
        if (tokenResult.IsFailure)
        {
            return Result.Failure(tokenResult.Error);
        }
        return Result.Success();
    }

    public async Task<Result> RegisterAsync(RegisterDto request)
    {
        var user = await userManager.FindByEmailAsync(request.Email) ??
                   await userManager.FindByNameAsync(request.Username);

        if (user != null)
        {
            return Result.Failure(Error.BadRequest(AuthErrors.UserAlreadyExists));
        }

        IdentityResult result = await userManager.CreateAsync(new()
        {
            UserName = request.Username,
            Email = request.Email
        }, request.Password);

        if (!result.Succeeded)
        {
            return Result.Failure(Error.BadRequest(new("One or more errors happened.", string.Join(",", result.Errors))));
        }
        return Result.Success();
    }
}


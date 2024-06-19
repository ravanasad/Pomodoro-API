using Application.Services.TokenService;
using Domain.Entities.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auths.Commands.LoginCommand;

public sealed class LoginCommandHandler(UserManager<AppUser> userManager,
                                        ITokenService tokenService) : IRequestHandler<LoginCommand, Result>
{
    private UserManager<AppUser> UserManager { get; } = userManager;
    private ITokenService TokenService { get; } = tokenService;

    public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await UserManager.FindByEmailAsync(request.EmailOrUsername) ?? await UserManager.FindByNameAsync(request.EmailOrUsername);

        if (user == null)
        {
            return Result.Failure("Invalid username or password.");
        }

        var result = await UserManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            return Result.Failure("Invalid username or password.");
        }

        var roles = await UserManager.GetRolesAsync(user);
        await TokenService.GenerateToken(user, roles);
        return Result.Success();
    }
}


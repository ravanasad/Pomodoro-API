using Application.Services.TokenService;
using Domain.Entities.Auth;
using Domain.Exceptions.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auths.Commands.LoginCommand;

public sealed class LoginCommandHandler(UserManager<AppUser> userManager,
                                        ITokenService tokenService) : IRequestHandler<LoginCommand>
{
    public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.EmailOrUsername) ?? await userManager.FindByNameAsync(request.EmailOrUsername);

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        var result = await userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            throw new InvalidPasswordException();
        }

        var roles = await userManager.GetRolesAsync(user);
        await tokenService.GenerateToken(user, roles);
    }
}


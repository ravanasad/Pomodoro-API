using Domain.Entities.Auth;
using Domain.Exceptions.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auths.Commands.RegisterCommand;

public sealed class RegisterCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<RegisterCommand, Result>
{
    private UserManager<AppUser> UserManager { get; } = userManager;

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email) ?? 
                   await userManager.FindByNameAsync(request.UserName);

        if (user != null)
        {
            return Result.Failure("User already exists.");
        }

        IdentityResult result = await userManager.CreateAsync(new()
        {
            UserName = request.UserName,
            Email = request.Email
        }, request.Password);

        if (!result.Succeeded)
        {
            return Result.Failure(result.Errors.Select(x => x.Description).ToList());
        }
        return Result.Success();
    }
}


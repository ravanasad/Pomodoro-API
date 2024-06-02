using Domain.Entities.Auth;
using Domain.Exceptions.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auths.Commands.RegisterCommand;

public sealed class RegisterCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<RegisterCommand>
{
    public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email)
            ?? await userManager.FindByNameAsync(request.UserName);

        if (user != null)
            throw new UserAlreadyExpcetion();

        IdentityResult result = await userManager.CreateAsync(new()
        {
            UserName = request.UserName,
            Email = request.Email
        }, request.Password);

        if (!result.Succeeded)
        {
            string errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new RegisterExceptions(errors);
        }

    }
}


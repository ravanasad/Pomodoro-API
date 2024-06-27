using Application.Services.AuthService.Local;
using MediatR;

namespace Application.Features.Auths.Commands.LoginCommand;

public sealed class LoginCommandHandler(ILocalAuthService localAuthService) : IRequestHandler<LoginCommand, Result>
{
    public ILocalAuthService LocalAuthService { get; } = localAuthService;

    public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await LocalAuthService.LoginAsync(new(request.EmailOrUsername, request.Password));
    }
}


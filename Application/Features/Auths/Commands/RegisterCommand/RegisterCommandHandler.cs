using Application.Services.AuthService.Local;
using MediatR;

namespace Application.Features.Auths.Commands.RegisterCommand;

public sealed class RegisterCommandHandler(ILocalAuthService localAuthService) : IRequestHandler<RegisterCommand, Result>
{
    private ILocalAuthService LocalAuthService { get; } = localAuthService;

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await LocalAuthService.RegisterAsync(new(request.Email, request.Username, request.Password));
    }
}


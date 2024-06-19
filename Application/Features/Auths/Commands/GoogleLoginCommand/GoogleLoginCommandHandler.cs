using Application.Services.AuthService.Google;
using MediatR;

namespace Application.Features.Auths.Commands.GoogleLoginCommand;

public sealed class GoogleLoginCommandHandler(IGoogleAuthService authService) : IRequestHandler<GoogleLoginCommand>
{
    public IGoogleAuthService AuthService { get; } = authService;

    public async Task Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
    {
        await AuthService.AuthenticateAsync(new(request.Id,request.IdToken, request.Email,request.UserName,request.Picture, "Google"));
    }
}

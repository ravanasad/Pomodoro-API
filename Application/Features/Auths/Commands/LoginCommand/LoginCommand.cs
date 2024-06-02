using MediatR;

namespace Application.Features.Auths.Commands.LoginCommand;

public sealed class LoginCommand : IRequest
{
    public string EmailOrUsername { get; set; }
    public string Password { get; set; }
}


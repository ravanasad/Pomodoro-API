using MediatR;

namespace Application.Features.Auths.Commands.RegisterCommand;

public sealed class RegisterCommand : IRequest
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}


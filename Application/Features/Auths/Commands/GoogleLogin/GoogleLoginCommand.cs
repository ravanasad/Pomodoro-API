using MediatR;

namespace Application.Features.Auths.Commands.GoogleLoginCommand;

public sealed class GoogleLoginCommand : IRequest
{
    public string Id { get; set; }
    public string IdToken { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Picture { get; set; }
}


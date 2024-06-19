using Application.Features.Auths.Commands.LoginCommand;
using Application.Features.Auths.Commands.RegisterCommand;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : BaseController
{
    [HttpPost("signup")]
    public async Task<IActionResult> Register(RegisterCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Login(LoginCommand request)
    {
        return Ok(await Mediator.Send(request));
    }

    [HttpGet]
    public IActionResult CurrentUser()
    {
        return Ok(HttpContext.User.Identity?.Name);
    }
}

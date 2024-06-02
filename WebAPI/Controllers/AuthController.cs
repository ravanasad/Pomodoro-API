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
        await Mediator.Send(request);
        return Ok("User Registered Successfully");
    }

    [HttpPost("signin")]
    public async Task<IActionResult> Login(LoginCommand request)
    {
        await Mediator.Send(request);
        return Ok("User Logined Successfully");
    }

    [HttpGet]
    public IActionResult CurrentUser()
    {
        return Ok(HttpContext.User.Identity?.Name);
    }
}

﻿using Application.Features.Auths.Commands.LoginCommand;
using Application.Features.Auths.Commands.RegisterCommand;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : BaseController
{
    [HttpPost("signup")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterCommand request)
    {
        var result = await Mediator.Send(request);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok();
    }

    [HttpPost("signin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginCommand request)
    {
        var result = await Mediator.Send(request);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok();
    }

    [HttpGet]
    public IActionResult CurrentUser()
    {
        return Ok(HttpContext.User.Identity?.Name);
    }
}

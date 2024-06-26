using Application.DTOs.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class BaseController : ControllerBase
{
    private IMediator? _mediator;
    protected IMediator Mediator =>
        _mediator ??= (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator))!;

    protected IActionResult FromResult<T>(Result<T> result)
    {
        if (result.IsFailure)
        {
            switch (result.Error.StatusCode)
            {
                case System.Net.HttpStatusCode.NotFound:
                    return NotFound(result.Error);
                case System.Net.HttpStatusCode.BadRequest:
                    return BadRequest(result.Error);
                case System.Net.HttpStatusCode.Forbidden:
                    return Forbid();
                case System.Net.HttpStatusCode.Unauthorized:
                    return Unauthorized();
                case System.Net.HttpStatusCode.Conflict:
                    return Conflict(result.Error);
                case System.Net.HttpStatusCode.NoContent:
                    return NoContent();
                case System.Net.HttpStatusCode.Created:
                    return Created("", result.Value);
                case System.Net.HttpStatusCode.MethodNotAllowed:
                    return StatusCode((int)System.Net.HttpStatusCode.MethodNotAllowed, result.Error);
                case System.Net.HttpStatusCode.UnsupportedMediaType:
                    return StatusCode((int)System.Net.HttpStatusCode.UnsupportedMediaType, result.Error);   
                case System.Net.HttpStatusCode.NotImplemented:
                    return StatusCode((int)System.Net.HttpStatusCode.NotImplemented, result.Error);
                case System.Net.HttpStatusCode.ServiceUnavailable:
                    return StatusCode((int)System.Net.HttpStatusCode.ServiceUnavailable, result.Error);
                case System.Net.HttpStatusCode.RequestTimeout:
                    return StatusCode((int)System.Net.HttpStatusCode.RequestTimeout, result.Error);
                default:
                    return Problem(result.Error.Desc);
            }
        }
        return Ok(result.Value);
    }

    protected IActionResult FromResult(Result result)
    {
        if (result.IsFailure)
        {
            switch (result.Error.StatusCode)
            {
                case System.Net.HttpStatusCode.NotFound:
                    return NotFound(result.Error);
                case System.Net.HttpStatusCode.BadRequest:
                    return BadRequest(result.Error);
                case System.Net.HttpStatusCode.Forbidden:
                    return Forbid();
                case System.Net.HttpStatusCode.Unauthorized:
                    return Unauthorized();
                case System.Net.HttpStatusCode.Conflict:
                    return Conflict(result.Error);
                case System.Net.HttpStatusCode.NoContent:
                    return NoContent();
                case System.Net.HttpStatusCode.Created:
                    return Created();
                case System.Net.HttpStatusCode.MethodNotAllowed:
                    return StatusCode((int)System.Net.HttpStatusCode.MethodNotAllowed, result.Error);
                case System.Net.HttpStatusCode.UnsupportedMediaType:
                    return StatusCode((int)System.Net.HttpStatusCode.UnsupportedMediaType, result.Error);
                case System.Net.HttpStatusCode.NotImplemented:
                    return StatusCode((int)System.Net.HttpStatusCode.NotImplemented, result.Error);
                case System.Net.HttpStatusCode.ServiceUnavailable:
                    return StatusCode((int)System.Net.HttpStatusCode.ServiceUnavailable, result.Error);
                case System.Net.HttpStatusCode.RequestTimeout:
                    return StatusCode((int)System.Net.HttpStatusCode.RequestTimeout, result.Error);
                default:
                    return Problem(result.Error.Desc);
            }
        }
        return Ok();
    }
}
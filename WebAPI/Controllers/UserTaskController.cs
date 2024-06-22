using Application.DTOs;
using Application.DTOs.Result;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserTaskController(IUserTaskService userTaskService) : BaseController
{
    private readonly IUserTaskService userTaskService = userTaskService;

    [HttpGet]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType(typeof(IEnumerable<UserTaskDto>))]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await userTaskService.GetTasksByUserId(cancellationToken);
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType(typeof(UserTaskDto))]
    public async Task<IActionResult> GetByid(int id, CancellationToken cancellationToken)
    {
        var result = await userTaskService.GetTaskById(id, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpGet("priority")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType(typeof(IEnumerable<UserTaskDto>))]
    public async Task<IActionResult> GetByPriority(CancellationToken cancellationToken)
    {
        var result = await userTaskService.GetTasksByUserIdGroupByPriorityList(cancellationToken);
        if (result.IsFailure) 
        {
            return NotFound(result.Error);
        }
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] CreateUserTaskDto userTaskDto, CancellationToken cancellationToken)
    {
        var result = await userTaskService.CreateTask(userTaskDto, cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Created();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put([FromBody] UpdateUserTaskDto userTaskDto, CancellationToken cancellationToken)
    {
        var result = await userTaskService.UpdateTask(userTaskDto, cancellationToken);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok("Task updated successfully.");
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await userTaskService.DeleteTask(id, cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }
        return Ok("Task deleted successfully");
    }

}

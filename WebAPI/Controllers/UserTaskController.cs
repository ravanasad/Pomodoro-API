using Application.DTOs;
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
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await userTaskService.GetTasksByUserId(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByid(int id, CancellationToken cancellationToken)
    {
        var result = await userTaskService.GetTaskById(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("priority")]
    public async Task<IActionResult> GetByPriority(CancellationToken cancellationToken)
    {
        var result = await userTaskService.GetTasksByUserIdGroupByPriority(cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUserTaskDto userTaskDto, CancellationToken cancellationToken)
    {
        await userTaskService.CreateTask(userTaskDto, cancellationToken);
        return Created();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateUserTaskDto userTaskDto, CancellationToken cancellationToken)
    {
        await userTaskService.UpdateTask(userTaskDto, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await userTaskService.DeleteTask(id, cancellationToken);
        return Ok();
    }

}

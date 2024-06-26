using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("[controller]")]
[ApiController]

public class TaskStepController : BaseController
{
    private readonly ITaskStepService _taskStepService;
    public TaskStepController(ITaskStepService taskStepService)
    {
        _taskStepService = taskStepService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int taskId, CancellationToken cancellationToken)
    {
        var result = await _taskStepService.GetTaskStepsByTaskId(taskId, cancellationToken);
        return FromResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _taskStepService.GetTaskStepById(id, cancellationToken);
        return FromResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateTaskStepDto request, CancellationToken cancellationToken)
    {
        var result = await _taskStepService.CreateTaskStep(request, cancellationToken);
        return FromResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateTaskStepDto request, CancellationToken cancellationToken)
    {
        var result = await _taskStepService.UpdateTaskStep(request, cancellationToken);
        return FromResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _taskStepService.DeleteTaskStep(id, cancellationToken);
        return FromResult(result);
    }
}

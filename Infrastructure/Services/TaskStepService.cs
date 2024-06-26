using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Services;
public sealed class TaskStepService(AppDbContext DbContext, IUserContext UserContext) : ITaskStepService
{
    private readonly DbSet<TaskStep> dbContext = DbContext.TaskSteps;
    private readonly IUserContext userContext = UserContext;

    public async Task<Result<IEnumerable<TaskStepDto>>> GetTaskStepsByTaskId(int taskId, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<TaskStep> taskSteps = await dbContext.AsNoTracking().Where(x => x.UserTaskId == taskId).ToListAsync(cancellationToken);
        List<TaskStepDto> taskStepDtos = taskSteps.Select(taskStep => new TaskStepDto(taskStep.Id, taskStep.Description, taskStep.IsCompleted)).ToList();
        return Result<IEnumerable<TaskStepDto>>.Success(taskStepDtos);
    }

    public async Task<Result<TaskStepDto>> GetTaskStepById(int id, CancellationToken cancellationToken)
    {
        TaskStep? taskStep = await dbContext.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (taskStep == null)
        {
            return Result<TaskStepDto>.Failure(Error.NotFound(TaskStepErrors.TaskStepNotFound));
        }
        return Result<TaskStepDto>.Success(new TaskStepDto(taskStep.Id, taskStep.Description, taskStep.IsCompleted));
    }

    public async Task<Result> CreateTaskStep(CreateTaskStepDto taskStep, CancellationToken cancellationToken)
    {
        var task = await DbContext.UserTasks.FirstOrDefaultAsync(x => x.Id == taskStep.UserTaskId, cancellationToken);
        if (task == null)
        {
            return Result.Failure(Error.NotFound(UserTaskErrors.UserTaskNotFound));
        }
        if (!task.AppUserId.Equals(userContext.UserId))
        {
            return Result.Failure(Error.Custom(HttpStatusCode.Forbidden, UserTaskErrors.UnauthorizedAccess));
        }
        TaskStep newTaskStep = new() 
        { 
            Description = taskStep.Description, 
            UserTaskId = taskStep.UserTaskId, 
            IsCompleted = taskStep.IsCompleted 
        };
        await dbContext.AddAsync(newTaskStep, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UpdateTaskStep(UpdateTaskStepDto taskStep, CancellationToken cancellationToken)
    { 
        TaskStep? taskStepToUpdate = await dbContext.Include(x=>x.UserTask).FirstOrDefaultAsync(x => x.Id == taskStep.Id, cancellationToken);
        if (taskStepToUpdate == null)
        {
            return Result.Failure(Error.NotFound(TaskStepErrors.TaskStepNotFound));
        }
        if (!taskStepToUpdate.UserTask.AppUserId.Equals(userContext.UserId))
        {
            return Result.Failure(Error.Custom(HttpStatusCode.Forbidden, UserTaskErrors.UnauthorizedAccess));
        }
        taskStepToUpdate.Description = taskStep.Description;
        taskStepToUpdate.IsCompleted = taskStep.IsCompleted;
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> DeleteTaskStep(int id, CancellationToken cancellationToken)
    {
        TaskStep? taskStepToDelete = await dbContext.Include(x => x.UserTask).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (taskStepToDelete == null)
        {
            return Result.Failure(Error.NotFound(TaskStepErrors.TaskStepNotFound));
        }
        if (!taskStepToDelete.UserTask.AppUserId.Equals(userContext.UserId))
        {
            return Result.Failure(Error.Custom(HttpStatusCode.Forbidden, UserTaskErrors.UnauthorizedAccess));
        }
        dbContext.Remove(taskStepToDelete);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> CompleteTaskStep(int id, CancellationToken cancellationToken)
    {
        TaskStep? taskStep = await dbContext.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (taskStep == null)
        {
            return Result.Failure(Error.NotFound(TaskStepErrors.TaskStepNotFound));
        }
        taskStep.IsCompleted = true;
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UncompleteTaskStep(int id, CancellationToken cancellationToken)
    {
        TaskStep? taskStep = await dbContext.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (taskStep == null)
        {
            return Result.Failure(Error.NotFound(TaskStepErrors.TaskStepNotFound));
        }
        taskStep.IsCompleted = false;
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}

using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Infrastructure.Services;

public sealed class UserTaskService(AppDbContext dbContext, IUserContext context) : IUserTaskService
{
    private readonly DbSet<UserTask> _userTasks = dbContext.UserTasks;
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IUserContext _userContext = context;

    public async Task<Result<UserTaskDto>> GetTaskById(int id, CancellationToken cancellationToken)
    {
        UserTask? userTask = await _userTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (userTask == null)
        {
            return Result<UserTaskDto>.Failure(Error.NotFound(UserTaskErrors.UserTaskNotFound));
        }
        if (userTask.AppUserId != _userContext.UserId)
        {
            return Result<UserTaskDto>.Failure(Error.InvalidRequest(UserTaskErrors.UnauthorizedAccess));
        }
        return Result<UserTaskDto>.Success(MapToUserTaskDto(userTask));
    }

    public async Task<Result<IEnumerable<UserTaskDto>>> GetTasksByUserId(CancellationToken cancellationToken)
    {
        var userTasks = await _userTasks.AsNoTracking().Where(x => x.AppUserId == _userContext.UserId).ToListAsync(cancellationToken);
        var userTaskDtos = userTasks.Select(userTask => MapToUserTaskDto(userTask)).ToList();
        return Result<IEnumerable<UserTaskDto>>.Success(userTaskDtos);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static UserTaskDto MapToUserTaskDto(UserTask userTask)
    {
        return new UserTaskDto(
            userTask.Id,
            userTask.Title,
            userTask.Description,
            userTask.Label,
            userTask.DueDate,
            userTask.Priority,
            userTask.IsCompleted
        );
    }

    public async Task<Result<UserTaskPriorityDto>> GetTasksByUserIdGroupByPriorityList(CancellationToken cancellationToken)
    {
        Result<IEnumerable<UserTaskDto>> userTasksResult = await GetTasksByUserId(cancellationToken);
        if (userTasksResult.IsFailure)
        {
            return Result<UserTaskPriorityDto>.Failure(userTasksResult.Error);
        }

        var userTasks = userTasksResult.Value.ToList();
        var tasks = new List<UserTaskListDto>
        {
            new(0,  "Must",   "Red",   []),
            new(1,  "Should", "Blue",  []),
            new(2,  "Could",  "Green", []),
            new(3,  "Won't",  "Grey",  [])
        };

        var taskDict = new Dictionary<TaskPriority, UserTaskListDto>
        {
            { TaskPriority.Must,   tasks[0] },
            { TaskPriority.Should, tasks[1] },
            { TaskPriority.Could,  tasks[2] },
            { TaskPriority.Wont,   tasks[3] }
        };

        int total = userTasks.Count;
        int completed = userTasks.Count(x => x.IsCompleted);

        foreach (var task in userTasks)
        {
            taskDict[task.TaskPriority].items.Add(task);
        }

        return Result<UserTaskPriorityDto>.Success(new UserTaskPriorityDto(total, total - completed, completed, tasks));
    }

    public async Task<Result> CreateTask(CreateUserTaskDto task, CancellationToken cancellationToken)
    {
        var userTask = new UserTask
        {
            AppUserId = _userContext.UserId,
            Title = task.Title,
            Description = task.Description,
            Label = task.Label,
            DueDate = task.DueDate,
            Priority = task.TaskPriority,
            IsCompleted = task.IsCompleted
        };

        await _userTasks.AddAsync(userTask, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> CreateTaskForTask(int taskId, CreateUserTaskDto task, CancellationToken cancellationToken)
    {
        UserTask? userTask = await _userTasks.FirstOrDefaultAsync(x => x.Id == taskId, cancellationToken);
        if (userTask == null)
        {
            return Result.Failure(Error.NotFound(UserTaskErrors.UserTaskNotFound));
        }
        var extendTask = new UserTask
        {
            AppUserId = _userContext.UserId,
            Title = task.Title,
            Description = task.Description,
            Label = task.Label,
            DueDate = task.DueDate,
            Priority = task.TaskPriority,
            IsCompleted = task.IsCompleted
        };
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UpdateTask(UpdateUserTaskDto task, CancellationToken cancellationToken)
    {
        var userTask = await _userTasks.FirstOrDefaultAsync(x => x.Id == task.Id, cancellationToken);
        if (userTask == null)
        {
            return Result.Failure(Error.NotFound(UserTaskErrors.UserTaskNotFound));
        }
        if (userTask.AppUserId != _userContext.UserId)
        {
            return Result.Failure(Error.InvalidRequest(UserTaskErrors.UnauthorizedAccess));
        }

        userTask.Title = task.Title;
        userTask.Description = task.Description;
        userTask.Label = task.Label;
        userTask.DueDate = task.DueDate;
        userTask.Priority = task.TaskPriority;
        userTask.IsCompleted = task.IsCompleted;

        _userTasks.Update(userTask);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteTask(int id, CancellationToken cancellationToken)
    {
        var userTask = await _userTasks.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (userTask == null)
        {
            return Result.Failure(Error.NotFound(UserTaskErrors.UserTaskNotFound));
        }
        if (userTask.AppUserId != _userContext.UserId)
        {
            return Result.Failure(Error.InvalidRequest(UserTaskErrors.UnauthorizedAccess));
        }

        _userTasks.Remove(userTask);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}

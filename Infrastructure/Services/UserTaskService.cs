using Application.Configurations;
using Application.DTOs;
using Application.Repositories;
using Application.Services;
using Domain.Enums;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Infrastructure.Services;

public sealed class UserTaskService(IUserTaskRepository UserTaskRepository, IUserContext context) : IUserTaskService
{
    private readonly IUserTaskRepository userTaskRepository = UserTaskRepository;

    public async Task<UserTaskDto> GetTaskById(int id, CancellationToken cancellationToken)
    {
        UserTask userTask = await userTaskRepository.GetByIdAsync(id, cancellationToken);
        if (userTask == null)
        {
            throw new Exception("Task not found");
        }
        if (userTask.AppUserId != context.UserId)
        {
            throw new Exception("Unauthorized access to task");
        }
        return new UserTaskDto(userTask.Id, userTask.Title, userTask.Description, userTask.Label, userTask.DueDate, userTask.Priority, userTask.IsCompleted);
    }

    public async Task<IEnumerable<UserTaskDto>> GetTasksByUserId(CancellationToken cancellationToken)
    {
        var userTasks = await userTaskRepository.GetWhereAsync(x=>x.AppUserId == context.UserId, cancellationToken);
        return userTasks.Select(userTask => new UserTaskDto(userTask.Id, userTask.Title, userTask.Description, userTask.Label, userTask.DueDate, userTask.Priority, userTask.IsCompleted));
    }

    public async Task<IReadOnlyDictionary<TaskPriority, IReadOnlyCollection<UserTaskDto>>> GetTasksByUserIdGroupByPriority(CancellationToken cancellationToken)
    {
        var userTasks = await GetTasksByUserId(cancellationToken);
        IReadOnlyDictionary<TaskPriority, IReadOnlyCollection<UserTaskDto>> keyValuePairs = userTasks
            .GroupBy(userTask => userTask.TaskPriority)
            .ToImmutableDictionary(group => group.Key, group => (IReadOnlyCollection<UserTaskDto>)group.ToList());

        return keyValuePairs;
    }

    public async Task<UserTaskPriorityDto> GetTasksByUserIdGroupByPriorityList(CancellationToken cancellationToken)
    {
        IEnumerable<UserTaskDto> userTasks = await GetTasksByUserId(cancellationToken);
        int id = 0;
        List<Tuple<TaskPriority, string, string>> tuples = [
            new (TaskPriority.Must, "Must", "Red"),
            new (TaskPriority.Should, "Should", "Blue"),
            new (TaskPriority.Could, "Could", "Green"),
            new (TaskPriority.Wont, "Wont", "Grey")
            ];
        int total = userTasks.Count();
        int completed = userTasks.Count(x => x.IsCompleted);
        List<UserTaskListDto> tasks = tuples.Aggregate(new List<UserTaskListDto>(), (acc, tuple) =>
        {
            var userTasksByPriority = userTasks.Where(userTask => userTask.TaskPriority == tuple.Item1).ToList();
            acc.Add(new UserTaskListDto(id++, tuple.Item2, tuple.Item3, userTasksByPriority));
            return acc;
        });

        return new(total, total - completed, completed, tasks);
    }

    public async Task CreateTask(CreateUserTaskDto task, CancellationToken cancellationToken)
    {
        UserTask userTask = new UserTask()
        {
            AppUserId = context.UserId,
            Title = task.Title,
            Description = task.Description,
            Label = task.Label,
            DueDate = task.DueDate,
            Priority = task.TaskPriority,
            IsCompleted = task.IsCompleted
        };
        await userTaskRepository.CreateAsync(userTask, cancellationToken);
        int a = await userTaskRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateTask(UpdateUserTaskDto task, CancellationToken cancellationToken)
    {
        UserTask userTask = new UserTask()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Label = task.Label,
            DueDate = task.DueDate,
            Priority = task.TaskPriority,
            IsCompleted = task.IsCompleted,
            AppUserId = context.UserId
        };
        await userTaskRepository.UpdateAsync(userTask, cancellationToken);
        await userTaskRepository.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteTask(int id, CancellationToken cancellationToken)
    {
        UserTask userTask = await userTaskRepository.GetByIdAsync(id, cancellationToken);
        if (userTask == null)
        {
            throw new Exception("Task not found");
        }
        if (userTask.AppUserId != context.UserId)
        {
            throw new Exception("Unauthorized access to task");
        }
        userTaskRepository.Delete(userTask, cancellationToken);
        await userTaskRepository.SaveChangesAsync(cancellationToken);
    }
}


using Application.Configurations;
using Application.DTOs;
using Application.Services;
using Domain.Enums;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Infrastructure.Services;

public sealed class UserTaskService(AppDbContext DbContext, IUserContext context) : IUserTaskService
{
    private readonly DbSet<UserTask> _context = DbContext.UserTasks;
    public async Task<UserTaskDto> GetTaskById(int id, CancellationToken cancellationToken)
    {
        UserTask? userTask = await _context.FirstOrDefaultAsync(x => x.Id == id);
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
        var userTasks = await _context.Where(x => x.AppUserId == context.UserId).ToListAsync();
        return userTasks.Select(userTask => new UserTaskDto(userTask.Id,
                                                            userTask.Title,
                                                            userTask.Description,
                                                            userTask.Label,
                                                            userTask.DueDate,
                                                            userTask.Priority,
                                                            userTask.IsCompleted));
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

        #region Method 1
        //int id = 0;
        //List<Tuple<TaskPriority, string, string>> tuples = [
        //    new (TaskPriority.Must, "Must", "Red"),
        //    new (TaskPriority.Should, "Should", "Blue"),
        //    new (TaskPriority.Could, "Could", "Green"),
        //    new (TaskPriority.Wont, "Wont", "Grey")
        //    ];
        //int total = userTasks.Count();
        //int completed = userTasks.Count(x => x.IsCompleted);
        //List<UserTaskListDto> tasks = tuples.Aggregate(new List<UserTaskListDto>(), (acc, tuple) =>
        //{
        //    var userTasksByPriority = userTasks.Where(userTask => userTask.TaskPriority == tuple.Item1).ToList();
        //    acc.Add(new UserTaskListDto(id++, tuple.Item2, tuple.Item3, userTasksByPriority));
        //    return acc;
        //});
        #endregion

        #region Method 2

        List<UserTaskListDto> tasks = new()
        {
            new UserTaskListDto(0, "Must", "Red", []),
            new UserTaskListDto(1, "Should", "Blue", []),
            new UserTaskListDto(2, "Could", "Green", []),
            new UserTaskListDto(3, "Would", "Grey", [])

        };
        Dictionary<TaskPriority, UserTaskListDto> taskDict = new()
        {
            { TaskPriority.Must, tasks[0] },
            { TaskPriority.Should, tasks[1] },
            { TaskPriority.Could, tasks[2] },
            { TaskPriority.Wont, tasks[3] }
        };
        int total = 0, completed = 0;
        foreach (UserTaskDto task in userTasks)
        {
            total++;
            if (task.IsCompleted)
            {
                completed++;
            }
            taskDict[task.TaskPriority].items.Add(task);
        }

        #endregion

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
        await _context.AddAsync(userTask, cancellationToken);
        _ = await DbContext.SaveChangesAsync(cancellationToken);
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
        _context.Update(userTask);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteTask(int id, CancellationToken cancellationToken)
    {
        UserTask? userTask = await _context.FirstOrDefaultAsync(x => x.Id == id);
        if (userTask == null)
        {
            throw new Exception("Task not found");
        }
        if (userTask.AppUserId != context.UserId)
        {
            throw new Exception("Unauthorized access to task");
        }
        _context.Remove(userTask);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}


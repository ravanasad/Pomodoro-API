using Application.DTOs;
using Domain.Enums;

namespace Application.Services;

public interface IUserTaskService
{
    Task CreateTask(CreateUserTaskDto task, CancellationToken cancellationToken);
    Task UpdateTask(UpdateUserTaskDto task, CancellationToken cancellationToken);
    Task DeleteTask(int id, CancellationToken cancellationToken);

    Task<UserTaskDto> GetTaskById(int id, CancellationToken cancellationToken);
    Task<IEnumerable<UserTaskDto>> GetTasksByUserId(CancellationToken cancellationToken);
    Task<IReadOnlyDictionary<TaskPriority, IReadOnlyCollection<UserTaskDto>>> GetTasksByUserIdGroupByPriority(CancellationToken cancellationToken);

    Task<UserTaskPriorityDto> GetTasksByUserIdGroupByPriorityList(CancellationToken cancellationToken);
}


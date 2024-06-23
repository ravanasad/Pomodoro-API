namespace Application.Services;

public interface IUserTaskService
{
    Task<Result> CreateTask(CreateUserTaskDto task, CancellationToken cancellationToken);
    Task<Result> CreateTaskForTask(int taskId, CreateUserTaskDto task, CancellationToken cancellationToken);

    Task<Result> UpdateTask(UpdateUserTaskDto task, CancellationToken cancellationToken);

    Task<Result> DeleteTask(int id, CancellationToken cancellationToken);

    Task<Result<UserTaskDto>> GetTaskById(int id, CancellationToken cancellationToken);

    Task<Result<IEnumerable<UserTaskDto>>> GetTasksByUserId(CancellationToken cancellationToken);

    Task<Result<UserTaskPriorityDto>> GetTasksByUserIdGroupByPriorityList(CancellationToken cancellationToken);
}

namespace Application.Services;

public interface ITaskStepService
{
    Task<Result<IEnumerable<TaskStepDto>>> GetTaskStepsByTaskId(int taskId, CancellationToken cancellationToken);
    Task<Result<TaskStepDto>> GetTaskStepById(int id, CancellationToken cancellationToken);
    Task<Result> CreateTaskStep(CreateTaskStepDto taskStep, CancellationToken cancellationToken);
    Task<Result> UpdateTaskStep(UpdateTaskStepDto taskStep, CancellationToken cancellationToken);
    Task<Result> DeleteTaskStep(int id, CancellationToken cancellationToken);
    Task<Result> CompleteTaskStep(int id, CancellationToken cancellationToken);
    Task<Result> UncompleteTaskStep(int id, CancellationToken cancellationToken);

}

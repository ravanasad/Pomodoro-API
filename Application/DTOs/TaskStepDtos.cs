namespace Application.DTOs;

public sealed record TaskStepDto(int Id, string Description, bool IsCompleted);

public sealed record CreateTaskStepDto(string Description, int UserTaskId, bool IsCompleted = false);

public sealed record UpdateTaskStepDto(int Id, string Description, bool IsCompleted);

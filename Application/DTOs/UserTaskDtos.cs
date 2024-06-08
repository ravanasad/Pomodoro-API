using Domain.Enums;

namespace Application.DTOs;


public sealed record CreateUserTaskDto(string Title, string Description, string Label, DateTime DueDate, TaskPriority TaskPriority = TaskPriority.Wont, bool IsCompleted = false);

public sealed record UpdateUserTaskDto(int Id, string Title, string Description, string Label, DateTime DueDate, TaskPriority TaskPriority, bool IsCompleted);

public sealed record class UserTaskDto(int Id, string Title, string Description, string Label, DateTime DueDate, TaskPriority TaskPriority, bool IsCompleted);

public sealed record class UserTaskListDto(int id, string title, string color, List<UserTaskDto> items);

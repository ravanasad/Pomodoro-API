using Domain.Enums;
using MediatR;

namespace Application.Features.UserTasks.Commands.UserTaskUpdate;

public sealed record UpdateUserTaskCommand(
    int Id,
    string Title,
    string Description,
    string Label,
    DateTime DueDate,
    TaskPriority Priority,
    int Status,
    bool IsCompleted
) : IRequest<Result>;

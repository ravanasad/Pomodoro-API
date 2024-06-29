using Domain.Enums;
using MediatR;

namespace Application.Features.UserTasks.Commands.UserTaskCreate;

// todo convert to record
public sealed record CreateUserTaskCommand(
    string Title,
    string Description,
    string Label,
    DateTime DueDate,
    TaskPriority Priority,
    int Status,
    int UserId,
    bool IsCompleted
) : IRequest<Result>;

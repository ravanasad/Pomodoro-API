using Domain.Enums;
using MediatR;

namespace Application.Features.UserTasks.Commands.UserTaskUpdate;

public sealed class UserTaskUpdateCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Label { get; set; }
    public DateTime DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    public int Status { get; set; }
    public bool IsCompleted { get; set; }
}
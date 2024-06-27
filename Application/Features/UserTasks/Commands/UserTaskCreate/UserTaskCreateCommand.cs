using Application.Services;
using Domain.Enums;
using MediatR;

namespace Application.Features.UserTasks.Commands.UserTaskCreate;
public class UserTaskCreateCommand : IRequest<Result>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Label { get; set; }
    public DateTime DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    public int Status { get; set; }
    public int UserId { get; set; }
    public bool IsCompleted { get; set; }
}


public class UserTaskCreateCommandHandler(IUserTaskService UserTaskService) : IRequestHandler<UserTaskCreateCommand, Result>
{
    private readonly IUserTaskService userTaskService = UserTaskService;

    public async Task<Result> Handle(UserTaskCreateCommand request, CancellationToken cancellationToken)
    {
        return await userTaskService.CreateTask(new(request.Title, request.Description, request.Label, request.DueDate, request.Priority, request.IsCompleted), cancellationToken);
    }
}

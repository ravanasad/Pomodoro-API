using Application.Services;
using MediatR;

namespace Application.Features.UserTasks.Commands.UserTaskCreate;

public class UserTaskCreateCommandHandler(IUserTaskService UserTaskService) : IRequestHandler<UserTaskCreateCommand, Result>
{
    private readonly IUserTaskService userTaskService = UserTaskService;

    public async Task<Result> Handle(UserTaskCreateCommand request, CancellationToken cancellationToken)
    {
        return await userTaskService.CreateTask(new(request.Title, request.Description, request.Label, request.DueDate, request.Priority, request.IsCompleted), cancellationToken);
    }
}

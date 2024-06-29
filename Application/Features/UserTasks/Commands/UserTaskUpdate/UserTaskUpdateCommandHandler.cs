using Application.Services;
using MediatR;

namespace Application.Features.UserTasks.Commands.UserTaskUpdate;

public sealed class UserTaskUpdateCommandHandler : IRequestHandler<UserTaskUpdateCommand, Result>
{
    private readonly IUserTaskService _userTaskService;

    public UserTaskUpdateCommandHandler(IUserTaskService userTaskService)
    {
        _userTaskService = userTaskService;
    }

    public async Task<Result> Handle(UserTaskUpdateCommand request, CancellationToken cancellationToken)
    {
        return await _userTaskService.UpdateTask(new(request.Id, request.Title, request.Description, request.Label, request.DueDate, request.Priority, request.IsCompleted), cancellationToken);
    }
}
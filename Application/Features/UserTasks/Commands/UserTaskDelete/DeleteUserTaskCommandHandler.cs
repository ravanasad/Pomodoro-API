using Application.Services;
using MediatR;

namespace Application.Features.UserTasks.Commands.UserTaskDelete;

public sealed class DeleteUserTaskCommandHandler(IUserTaskService UserTaskService) : IRequestHandler<DeleteUserTaskCommand, Result>
{
    private readonly IUserTaskService userTaskService = UserTaskService;

    public async Task<Result> Handle(DeleteUserTaskCommand request, CancellationToken cancellationToken)
    {
        return await userTaskService.DeleteTask(request.Id, cancellationToken);
    }
}

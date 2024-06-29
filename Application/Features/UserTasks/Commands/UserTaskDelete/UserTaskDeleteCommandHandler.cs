using Application.Services;
using MediatR;

namespace Application.Features.UserTasks.Commands.UserTaskDelete;

public sealed class UserTaskDeleteCommandHandler(IUserTaskService UserTaskService) : IRequestHandler<UserTaskDeleteCommand, Result>
{
    private readonly IUserTaskService userTaskService = UserTaskService;

    public async Task<Result> Handle(UserTaskDeleteCommand request, CancellationToken cancellationToken)
    {
        return await userTaskService.DeleteTask(request.Id, cancellationToken);
    }
}

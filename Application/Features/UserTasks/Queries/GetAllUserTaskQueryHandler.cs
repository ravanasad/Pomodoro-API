using Application.Services;
using MediatR;

namespace Application.Features.UserTasks.Queries;

public sealed class  GetAllUserTaskQueryHandler(IUserTaskService UserTaskService) : IRequestHandler<GetAllUserTaskQuery, Result<IEnumerable<UserTaskDto>>>
{
    private readonly IUserTaskService userTaskService = UserTaskService;

    public async Task<Result<IEnumerable<UserTaskDto>>> Handle(GetAllUserTaskQuery request, CancellationToken cancellationToken)
    {
        return await userTaskService.GetTasksByUserId(cancellationToken);
    }
}
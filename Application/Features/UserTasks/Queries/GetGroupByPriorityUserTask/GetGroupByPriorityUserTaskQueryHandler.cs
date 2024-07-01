using Application.Services;
using MediatR;

namespace Application.Features.UserTasks.Queries.GetGroupByPriorityUserTask;

public sealed class GetGroupByPriorityUserTaskQueryHandler(IUserTaskService UserTaskService) : IRequestHandler<GetGroupByPriorityUserTaskQuery, Result<UserTaskPriorityDto>>
{
    private readonly IUserTaskService userTaskService = UserTaskService;

    public async Task<Result<UserTaskPriorityDto>> Handle(GetGroupByPriorityUserTaskQuery request, CancellationToken cancellationToken)
    {
        return await userTaskService.GetTasksByUserIdGroupByPriorityList(cancellationToken);
    }
}
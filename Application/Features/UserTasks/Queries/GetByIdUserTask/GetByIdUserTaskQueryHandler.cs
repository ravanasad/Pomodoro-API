using Application.Services;
using MediatR;

namespace Application.Features.UserTasks.Queries.GetByIdUserTask;

public sealed class GetByIdUserTaskQueryHandler(IUserTaskService UserTaskService) :         
                            IRequestHandler<GetByIdUserTaskQuery, Result<UserTaskDto>>
{
    private readonly IUserTaskService userTaskService = UserTaskService;

    public async Task<Result<UserTaskDto>> Handle(GetByIdUserTaskQuery request, CancellationToken cancellationToken)
    {
        return await userTaskService.GetTaskById(request.Id, cancellationToken);
    }
    
}
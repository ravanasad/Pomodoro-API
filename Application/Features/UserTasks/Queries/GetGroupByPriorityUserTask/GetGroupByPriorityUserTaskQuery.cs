using MediatR;

namespace Application.Features.UserTasks.Queries.GetGroupByPriorityUserTask;

public sealed record GetGroupByPriorityUserTaskQuery() 
    : IRequest<Result<UserTaskPriorityDto>>;

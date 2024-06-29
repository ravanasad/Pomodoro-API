using MediatR;

namespace Application.Features.UserTasks.Queries.GetAllUserTask;
public sealed class GetAllUserTaskQuery : IRequest<Result<IEnumerable<UserTaskDto>>>
{

}
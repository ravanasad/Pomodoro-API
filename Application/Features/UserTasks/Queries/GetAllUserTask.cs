using MediatR;

namespace Application.Features.UserTasks.Queries;
public sealed class GetAllUserTaskQuery : IRequest<Result<IEnumerable<UserTaskDto>>>
{ 

}
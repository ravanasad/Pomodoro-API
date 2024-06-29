using MediatR;

namespace Application.Features.UserTasks.Queries.GetByIdUserTask;
public sealed record GetByIdUserTaskQuery (int Id) : IRequest<Result<UserTaskDto>>;

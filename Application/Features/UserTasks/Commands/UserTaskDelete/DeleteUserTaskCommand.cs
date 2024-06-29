using MediatR;

namespace Application.Features.UserTasks.Commands.UserTaskDelete;

public sealed record DeleteUserTaskCommand(int Id) : IRequest<Result>;
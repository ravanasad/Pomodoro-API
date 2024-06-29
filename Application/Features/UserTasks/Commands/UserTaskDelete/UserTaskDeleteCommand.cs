using MediatR;

namespace Application.Features.UserTasks.Commands.UserTaskDelete;

public sealed record UserTaskDeleteCommand(int Id) : IRequest<Result>;
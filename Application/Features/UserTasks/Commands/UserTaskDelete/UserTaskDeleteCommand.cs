using MediatR;

namespace Application.Features.UserTasks.Commands.UserTaskDelete;

public sealed class UserTaskDeleteCommand : IRequest<Result>
{
    public int Id { get; set; }
}
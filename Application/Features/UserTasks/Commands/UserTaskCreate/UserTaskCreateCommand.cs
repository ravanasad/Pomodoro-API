﻿using Domain.Enums;
using MediatR;

namespace Application.Features.UserTasks.Commands.UserTaskCreate;

public class UserTaskCreateCommand : IRequest<Result>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Label { get; set; }
    public DateTime DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    public int Status { get; set; }
    public int UserId { get; set; }
    public bool IsCompleted { get; set; }
}
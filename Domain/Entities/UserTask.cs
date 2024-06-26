namespace Domain.Entities;

public sealed class UserTask : BaseEntity
{
    public string Title { get; set; } = "New Task";
    public string Description { get; set; } = "";
    public string Label { get; set; } = "";
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; } = false;
    public TaskPriority Priority { get; set; } = TaskPriority.Wont;
    public HashSet<TaskStep> Steps { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}

public sealed class TaskStep : BaseEntity
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; } = false;
    public int UserTaskId { get; set; }
    public UserTask UserTask { get; set; }
}

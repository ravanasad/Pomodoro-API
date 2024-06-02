using System.Text.Json.Serialization;

namespace Domain.Entities;

public sealed class UserTask : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; } = "";
    public string Label { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; } = false;
    public TaskPriority Priority { get; set; } = TaskPriority.Wont;
    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}

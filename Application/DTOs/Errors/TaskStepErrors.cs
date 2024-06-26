namespace Application.DTOs.Errors;

public static class TaskStepErrors
{
    public static ErrorDesc TaskStepNotFound => new("Task step not found", "The task step you are looking for does not exist.");
    public static ErrorDesc TaskStepAlreadyCompleted => new("Task step already completed", "The task step you are trying to complete has already been marked as completed.");
    public static ErrorDesc TaskStepNotCompleted => new("Task step not completed", "The task step you are trying to mark as incomplete has not been marked as completed.");
    public static ErrorDesc TaskStepNotUpdated => new("Task step not updated", "The task step you are trying to update could not be updated. Please try again.");
    public static ErrorDesc TaskStepNotCreated => new("Task step not created", "The task step you are trying to create could not be created. Please try again.");
    public static ErrorDesc TaskStepNotDeleted => new("Task step not deleted", "The task step you are trying to delete could not be deleted. Please try again.");

}

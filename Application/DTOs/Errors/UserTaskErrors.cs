namespace Domain.Errors;
public static class UserTaskErrors
{
    public static ErrorDesc UserTaskNotFound => new("User Task not found", "The specified user task could not be found. Please ensure the task ID is correct and try again.");
    public static ErrorDesc UnauthorizedAccess => new("Unauthorized access to task", "You do not have the necessary permissions to access this task. Please contact your administrator if you believe this is an error.");
}

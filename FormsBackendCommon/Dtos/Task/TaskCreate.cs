namespace FormsBackendCommon.Dtos.Task;

public record TaskCreate(int UserId, string Title, string Description, DateTime DueDate);

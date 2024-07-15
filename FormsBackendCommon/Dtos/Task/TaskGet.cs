namespace FormsBackendCommon.Dtos.Task;

public record TaskGet(int Id, string Title, string Description, DateTime CreationDate, DateTime ModificationDate, DateTime DueDate);

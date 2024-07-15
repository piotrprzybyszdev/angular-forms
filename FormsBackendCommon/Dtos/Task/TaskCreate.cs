namespace FormsBackendCommon.Dtos.Task;

public record TaskCreate(string UserGuid, string Title, string Description, DateTime DueDate);

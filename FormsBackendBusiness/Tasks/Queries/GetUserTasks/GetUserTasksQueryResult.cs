namespace FormsBackendBusiness.Tasks.Queries.GetUserTasks;

public record TaskGet(int Id, string Title, string Description, DateTime CreationDate, DateTime ModificationDate, DateTime DueDate);

public class GetUserTasksQueryResult
{
    public List<TaskGet> Tasks { get; init; } = default!;
}

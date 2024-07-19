namespace FormsBackendBusiness.Tasks.Queries.GetUserTasks;

public class TaskGet
{
    public int Id { get; init; }
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public DateTime CreationDate { get; init; }
    public DateTime ModificationDate { get; init; }
    public DateTime DueDate { get; init; }
}

public class GetUserTasksQueryResult
{
    public List<TaskGet> Tasks { get; init; } = default!;
}

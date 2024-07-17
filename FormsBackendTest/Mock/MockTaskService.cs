using FormsBackendCommon.Dtos.Task;
using FormsBackendCommon.Interface;

namespace FormBackendTest.Mock;

public enum TaskServiceOperationType
{
    Create, Update, Delete, DeleteUser, GetByUser
};

public record TaskServiceOperation(TaskServiceOperationType OperationType, List<object> Arguments);

public class MockTaskService : ITaskService
{
    public List<TaskServiceOperation> Operations { get; } = [];

    public Task<int> CreateTaskAsync(TaskCreate taskCreate)
    {
        Operations.Add(new TaskServiceOperation(TaskServiceOperationType.Create, [taskCreate]));
        return Task.FromResult(0);
    }

    public Task UpdateTaskAsync(TaskUpdate taskUpdate)
    {
        Operations.Add(new TaskServiceOperation(TaskServiceOperationType.Update, [taskUpdate]));
        return Task.CompletedTask;
    }

    public Task DeleteTaskAsync(int id)
    {
        Operations.Add(new TaskServiceOperation(TaskServiceOperationType.Delete, [id]));
        return Task.CompletedTask;
    }

    public Task DeleteUserTasksAsync(int userId)
    {
        Operations.Add(new TaskServiceOperation(TaskServiceOperationType.DeleteUser, [userId]));
        return Task.CompletedTask;
    }

    public Task<List<TaskGet>> GetTasksByUserIdAsync(int userId)
    {
        Operations.Add(new TaskServiceOperation(TaskServiceOperationType.GetByUser, [userId]));
        return Task.FromResult<List<TaskGet>>([]);
    }
}

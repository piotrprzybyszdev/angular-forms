using FormsBackendCommon.Dtos.Task;

namespace FormsBackendCommon.Interface;

public interface ITaskService
{
    Task<int> CreateTaskAsync(TaskCreate taskCreate);
    Task UpdateTaskAsync(TaskUpdate taskUpdate);
    Task DeleteTaskAsync(int id);
    Task DeleteUserTasksAsync(string userId);
    Task<List<TaskGet>> GetTasksByUserIdAsync(string userId);
}

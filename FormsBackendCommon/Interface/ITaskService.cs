using FormsBackendCommon.Dtos.Task;

namespace FormsBackendCommon.Interface;

public interface ITaskService
{
    Task<int> CreateTaskAsync(TaskCreate taskCreate);
    Task UpdateTaskAsync(TaskUpdate taskUpdate);
    Task DeleteTaskAsync(int id);
    Task DeleteUserTasksAsync(UserTasksDelete userTasksDelete);
    Task<TaskGet> GetTaskByIdAsync(int id);
    Task<List<TaskGet>> GetTasksByUserIdAsync(string userId);
}

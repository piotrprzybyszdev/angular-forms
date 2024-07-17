using FormsBackendCommon.Model;

namespace FormsBackendCommon.Interface;

public interface ITaskRepository
{
    Task<int> InsertAsync(TaskModel task);
    Task Update(TaskModel task);
    Task<List<TaskModel>> GetAsync();
    Task<TaskModel?> GetByIdAsync(int id);
    Task<List<TaskModel>> GetByUserIdAsync(string userId);
    Task Delete(TaskModel task);
    Task DeleteByUserIdAsync(string userId);
    Task SaveChangesAsync();
}

using FormsBackendCommon.Model;

namespace FormsBackendCommon.Interface;

public interface ITaskRepository
{
    Task<int> InsertAsync(TaskModel task);
    Task Update(TaskModel task);
    Task<List<TaskModel>> GetAsync();
    Task<TaskModel?> GetByIdAsync(int id);
    Task<List<TaskModel>> GetByUserIdAsync(int userId);
    Task Delete(TaskModel task);
    Task DeleteByUserIdAsync(int userId);
    Task SaveChangesAsync();
}

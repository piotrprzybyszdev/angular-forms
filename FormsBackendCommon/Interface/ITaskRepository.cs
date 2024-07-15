using FormsBackendCommon.Model;

namespace FormsBackendCommon.Interface;

public interface ITaskRepository
{
    Task<int> InsertAsync(TaskModel task);
    void Update(TaskModel task);
    Task<List<TaskModel>> GetAsync();
    Task<TaskModel?> GetByIdAsync(int id);
    Task<List<TaskModel>> GetByUserIdAsync(string userId);
    void Delete(TaskModel task);
    Task DeleteByUserIdAsync(string userId);
    Task SaveChangesAsync();
}

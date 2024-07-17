using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;

namespace FormBackendTest.Mock;

public enum TaskRepositoryOperationType
{
    Insert, Update, Delete, DeleteByUserId,
    Get, GetById, GetByUserId, SaveChanges
};

public record TaskRepositoryOperation(TaskRepositoryOperationType OperationType, List<object> Arguments);

public class MockTaskRepository : ITaskRepository
{
    public List<TaskRepositoryOperation> Operations { get; } = [];

    public List<TaskModel> Tasks { get; set; } = [];

    public async Task<int> InsertAsync(TaskModel task)
    {
        Operations.Add(new TaskRepositoryOperation(TaskRepositoryOperationType.Insert, [task]));
        return await Task.FromResult(task.Id);
    }

    public async Task Update(TaskModel task)
    {
        Operations.Add(new TaskRepositoryOperation(TaskRepositoryOperationType.Update, [task]));
        await Task.CompletedTask;
    }

    public async Task Delete(TaskModel task)
    {
        Operations.Add(new TaskRepositoryOperation(TaskRepositoryOperationType.Delete, [task]));
        await Task.CompletedTask;
    }

    public async Task DeleteByUserIdAsync(int userId)
    {
        Operations.Add(new TaskRepositoryOperation(TaskRepositoryOperationType.DeleteByUserId, [userId]));
        await Task.CompletedTask;
    }

    public async Task<List<TaskModel>> GetAsync()
    {
        Operations.Add(new TaskRepositoryOperation(TaskRepositoryOperationType.Get, []));
        return await Task.FromResult(Tasks);
    }

    public async Task<TaskModel?> GetByIdAsync(int id)
    {
        Operations.Add(new TaskRepositoryOperation(TaskRepositoryOperationType.GetById, [id]));
        return await Task.FromResult(Tasks.Find(task => task.Id == id));
    }

    public async Task<List<TaskModel>> GetByUserIdAsync(int userId)
    {
        Operations.Add(new TaskRepositoryOperation(TaskRepositoryOperationType.GetByUserId, [userId]));
        return await Task.FromResult(Tasks.Where(task => task.User.Id == userId).ToList());
    }

    public async Task SaveChangesAsync()
    {
        Operations.Add(new TaskRepositoryOperation(TaskRepositoryOperationType.SaveChanges, []));
        await Task.CompletedTask;
    }
}
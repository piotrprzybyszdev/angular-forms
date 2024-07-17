using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;

namespace FormBackendTest.Mock;

public enum UserRepositoryOperationType
{
    Insert, Update, Delete, Get, GetById, SaveChanges
};

public record UserRepositoryOperation(UserRepositoryOperationType OperationType, List<object> Arguments);

public class MockUserRepository : IUserRepository
{
    public List<UserRepositoryOperation> Operations { get; } = [];

    public List<UserModel> Users { get; set; } = [];

    public async Task<int> InsertAsync(UserModel user)
    {
        Operations.Add(new UserRepositoryOperation(UserRepositoryOperationType.Insert, [user]));
        return await Task.FromResult(user.Id);
    }

    public async Task UpdateAsync(UserModel user)
    {
        Operations.Add(new UserRepositoryOperation(UserRepositoryOperationType.Update, [user]));
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(UserModel user)
    {
        Operations.Add(new UserRepositoryOperation(UserRepositoryOperationType.Delete, [user]));
        await Task.CompletedTask;
    }

    public async Task<List<UserModel>> GetAsync()
    {
        Operations.Add(new UserRepositoryOperation(UserRepositoryOperationType.Get, []));
        return await Task.FromResult(Users);
    }

    public async Task<UserModel?> GetyByIdAsync(int id)
    {
        Operations.Add(new UserRepositoryOperation(UserRepositoryOperationType.GetById, [id]));
        return await Task.FromResult(Users.Find(user => user.Id == id));
    }

    public async Task SaveChangesAsync()
    {
        Operations.Add(new UserRepositoryOperation(UserRepositoryOperationType.SaveChanges, []));
        await Task.CompletedTask;
    }
}

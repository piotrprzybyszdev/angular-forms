using FormsBackendCommon.Model;

namespace FormsBackendCommon.Interface;

public interface IUserRepository
{
    Task<int> InsertAsync(UserModel user);
    Task UpdateAsync(UserModel user);
    Task<List<UserModel>> GetAsync();
    Task<UserModel?> GetyByIdAsync(int id);
    Task DeleteAsync(UserModel user);
    Task SaveChangesAsync();
}

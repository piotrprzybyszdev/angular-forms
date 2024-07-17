using FormsBackendCommon.Dtos.User;

namespace FormsBackendCommon.Interface;

public interface IUserService
{
    Task<int> CreateUserAsync(UserCreate userCreate);
    Task UpdateUserAsync(UserUpdate userUpdate);
    Task DeleteUserAsync(int id);
    Task<UserGet?> GetUserById(int id);
    Task<List<UserGet>> GetUsersAsync();
}

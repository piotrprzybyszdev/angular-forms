using FormsBackendCommon.Dtos.User;

namespace FormsBackendCommon.Interface;

public interface IUserService
{
    Task<string> CreateUserAsync(UserCreate userCreate);
    Task UpdateUserAsync(UserUpdate userUpdate);
    Task DeleteUserAsync(string guid);
    Task<UserGet?> GetUserById(string guid);
    Task<List<UserGet>> GetUsersAsync();
}

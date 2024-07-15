using FormsBackendCommon.Dtos.User;

namespace FormsBackendCommon.Interface;

public interface IUserService
{
    Task<object> CreateUserAsync(UserCreate userCreate);
    Task<object?> UpdateUserAsync(UserUpdate userUpdate);
    Task DeleteUserAsync(string guid);
    Task<UserGet?> GetUserById(string guid);
    Task<List<UserGet>> GetUsersAsync();
}

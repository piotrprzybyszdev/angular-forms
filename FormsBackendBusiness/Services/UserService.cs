using AutoMapper;
using FormsBackendCommon.Dtos.Task;
using FormsBackendCommon.Dtos.User;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using Microsoft.AspNetCore.Identity;

namespace FormsBackendBusiness.Services;

public class UserService(UserManager<ApplicationUser> userManager,
    ITaskService taskService, IMapper mapper) : IUserService
{
    public async Task<object> CreateUserAsync(UserCreate userCreate)
    {
        var user = mapper.Map<ApplicationUser>(userCreate);
        return await userManager.CreateAsync(user, userCreate.Password);
    }

    public async Task DeleteUserAsync(string guid)
    {
        var user = await userManager.FindByIdAsync(guid);
        if (user == null) return;
        await taskService.DeleteUserTasksAsync(new UserTasksDelete(guid));
        await userManager.DeleteAsync(user);
    }

    public async Task<object?> UpdateUserAsync(UserUpdate userUpdate)
    {
        var user = await userManager.FindByIdAsync(userUpdate.Guid);
        if (user == null) return null;

        user.UserName = userUpdate.Email;
        user.Email = userUpdate.Email;
        user.FirstName = userUpdate.FirstName;
        user.LastName = userUpdate.LastName;

        return await userManager.UpdateAsync(user);
    }

    public async Task<UserGet?> GetUserById(string guid)
    {
        var user = await userManager.FindByIdAsync(guid);
        return mapper.Map<UserGet>(user);
    }

    public async Task<List<UserGet>> GetUsersAsync()
    {
        var users = mapper.Map<List<UserGet>>(userManager.Users.ToList());
        return await Task.FromResult(users);
    }
}

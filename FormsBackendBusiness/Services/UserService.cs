using AutoMapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendBusiness.Validation;
using FormsBackendCommon.Dtos.User;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FormsBackendBusiness.Services;

public class UserService(UserManager<ApplicationUser> userManager,
    ITaskService taskService, IMapper mapper, UserCreateValidator userCreateValidator,
    UserUpdateValidator userUpdateValidator) : IUserService
{
    public async Task<string> CreateUserAsync(UserCreate userCreate)
    {
        var validationResult = await userCreateValidator.ValidateAsync(userCreate);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        var user = mapper.Map<ApplicationUser>(userCreate);
        var result = await userManager.CreateAsync(user, userCreate.Password);
        if (result.Succeeded == false) throw new ValidationFailedException(result.Errors);
        return user.Id;
    }

    public async Task DeleteUserAsync(string guid)
    {
                var user = await userManager.FindByIdAsync(guid) ??
            throw new UserNotFoundException(guid);
        await taskService.DeleteUserTasksAsync(guid);
        await userManager.DeleteAsync(user);
    }

    public async Task UpdateUserAsync(UserUpdate userUpdate)
    {
        var validationResult = await userUpdateValidator.ValidateAsync(userUpdate);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        var user = await userManager.FindByIdAsync(userUpdate.Id) ??
            throw new UserNotFoundException(userUpdate.Id);

        user.UserName = userUpdate.Email;
        user.Email = userUpdate.Email;
        user.FirstName = userUpdate.FirstName;
        user.LastName = userUpdate.LastName;

        await userManager.UpdateAsync(user);
    }

    public async Task<UserGet?> GetUserById(string guid)
    {
        var user = await userManager.FindByIdAsync(guid) ??
            throw new UserNotFoundException(guid);
        return mapper.Map<UserGet>(user);
    }

    public async Task<List<UserGet>> GetUsersAsync()
    {
        return mapper.Map<List<UserGet>>(await userManager.Users.ToListAsync());
    }
}

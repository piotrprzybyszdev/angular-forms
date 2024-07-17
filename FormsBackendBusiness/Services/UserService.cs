using AutoMapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendBusiness.Validation;
using FormsBackendCommon.Dtos.User;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;

namespace FormsBackendBusiness.Services;

public class UserService(IUserRepository userRepository,
    ITaskService taskService, IMapper mapper, UserCreateValidator userCreateValidator,
    UserUpdateValidator userUpdateValidator) : IUserService
{
    public async Task<string> CreateUserAsync(UserCreate userCreate)
    {
        var validationResult = await userCreateValidator.ValidateAsync(userCreate);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        if ((await userRepository.GetAsync()).Any(user => user.Email == userCreate.Email))
            throw new ValidationFailedException([new ValidationError("Email validation failed", "User with this email already exists")]);

        var user = mapper.Map<ApplicationUser>(userCreate);
        var id = await userRepository.InsertAsync(user);
        await userRepository.SaveChangesAsync();
        return id;
    }

    public async Task DeleteUserAsync(string id)
    {
        var user = await userRepository.GetyByIdAsync(id) ??
            throw new UserNotFoundException(id);
        await taskService.DeleteUserTasksAsync(id);
        await userRepository.DeleteAsync(user);
        await userRepository.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(UserUpdate userUpdate)
    {
        var validationResult = await userUpdateValidator.ValidateAsync(userUpdate);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        var user = await userRepository.GetyByIdAsync(userUpdate.Id) ??
            throw new UserNotFoundException(userUpdate.Id);

        user.UserName = userUpdate.Email;
        user.Email = userUpdate.Email;
        user.FirstName = userUpdate.FirstName;
        user.LastName = userUpdate.LastName;

        await userRepository.UpdateAsync(user);
        await userRepository.SaveChangesAsync();
    }

    public async Task<UserGet?> GetUserById(string id)
    {
        var user = await userRepository.GetyByIdAsync(id) ??
            throw new UserNotFoundException(id);
        return mapper.Map<UserGet>(user);
    }

    public async Task<List<UserGet>> GetUsersAsync()
    {
        return mapper.Map<List<UserGet>>(await userRepository.GetAsync());
    }
}

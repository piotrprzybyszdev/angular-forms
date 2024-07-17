using AutoMapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendBusiness.Validation;
using FormsBackendCommon.Dtos.Task;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;

namespace FormsBackendBusiness.Services;

public class TaskService(ITaskRepository taskRepository,
    IUserRepository userRepository, IMapper mapper,
    TaskCreateValidator taskCreateValidator, TaskUpdateValidator taskUpdateValidator)
    : ITaskService
{
    public async Task<int> CreateTaskAsync(TaskCreate taskCreate)
    {
        var validationResult = await taskCreateValidator.ValidateAsync(taskCreate);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        var user = await userRepository.GetyByIdAsync(taskCreate.UserGuid) ??
            throw new UserNotFoundException(taskCreate.UserGuid);

        var task = mapper.Map<TaskModel>(taskCreate);
        task.Account = user;
        task.CreationDate = DateTime.Now;
        task.ModificationDate = DateTime.Now;

        await taskRepository.InsertAsync(task);
        await taskRepository.SaveChangesAsync();
        return task.Id;
    }

    public async Task UpdateTaskAsync(TaskUpdate taskUpdate)
    {
        var validationResult = await taskUpdateValidator.ValidateAsync(taskUpdate);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        var task = await taskRepository.GetByIdAsync(taskUpdate.Id) ??
            throw new TaskNotFoundException(taskUpdate.Id);

        task.Title = taskUpdate.Title;
        task.Description = taskUpdate.Description;
        task.DueDate = taskUpdate.DueDate;
        task.ModificationDate = DateTime.Now;

        await taskRepository.Update(task);
        await taskRepository.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await taskRepository.GetByIdAsync(id) ??
            throw new TaskNotFoundException(id);

        await taskRepository.Delete(task);
        await taskRepository.SaveChangesAsync();
    }

    public async Task DeleteUserTasksAsync(string UserGuid)
    {
        await taskRepository.DeleteByUserIdAsync(UserGuid);
    }

    public async Task<List<TaskGet>> GetTasksByUserIdAsync(string userId)
    {
        return mapper.Map<List<TaskGet>>(await taskRepository.GetByUserIdAsync(userId));
    }
}

using AutoMapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendBusiness.Validation;
using FormsBackendCommon.Dtos.Task;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;

namespace FormsBackendBusiness.Services;

public class TaskService(IGenericRepository<TaskModel> taskRepository,
    IGenericRepository<UserModel> userRepository, IMapper mapper,
    TaskCreateValidator taskCreateValidator, TaskUpdateValidator taskUpdateValidator)
    : ITaskService
{
    public async Task<int> CreateTaskAsync(TaskCreate taskCreate)
    {
        var validationResult = await taskCreateValidator.ValidateAsync(taskCreate);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        var user = await userRepository.GetByIdAsync(taskCreate.UserId) ??
            throw new UserNotFoundException(taskCreate.UserId);

        var task = mapper.Map<TaskModel>(taskCreate);
        task.User = user;
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

        await taskRepository.UpdateAsync(task);
        await taskRepository.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await taskRepository.GetByIdAsync(id) ??
            throw new TaskNotFoundException(id);

        await taskRepository.DeleteAsync(task);
        await taskRepository.SaveChangesAsync();
    }

    public async Task DeleteUserTasksAsync(int userId)
    {
        var tasks = await taskRepository.GetFilteredAsync(
            [task => task.User.Id == userId]
        );

        foreach (var task in tasks)
            await taskRepository.DeleteAsync(task);
    }

    public async Task<List<TaskGet>> GetTasksByUserIdAsync(int userId)
    {
        return mapper.Map<List<TaskGet>>(await taskRepository.GetFilteredAsync(
            [task => task.User.Id == userId]
        ));
    }
}

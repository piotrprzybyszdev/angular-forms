using AutoMapper;
using FormsBackendCommon.Dtos.Task;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using Microsoft.AspNetCore.Identity;

namespace FormsBackendBusiness.Services;

public class TaskService(ITaskRepository taskRepository,
    UserManager<ApplicationUser> userManager, IMapper mapper) : ITaskService
{
    public async Task<int> CreateTaskAsync(TaskCreate taskCreate)
    {
        var task = mapper.Map<TaskModel>(taskCreate);
        var user = await userManager.FindByIdAsync(taskCreate.UserGuid);

        if (user == null) return -1;
        task.Account = user;
        task.CreationDate = DateTime.Now;
        task.ModificationDate = DateTime.Now;

        await taskRepository.InsertAsync(task);
        await taskRepository.SaveChangesAsync();
        return task.Id;
    }

    public async Task UpdateTaskAsync(TaskUpdate taskUpdate)
    {
        var task = await taskRepository.GetByIdAsync(taskUpdate.Id);
        if (task == null) return;
        task.Title = taskUpdate.Title;
        task.Description = taskUpdate.Description;
        task.DueDate = taskUpdate.DueDate;
        task.ModificationDate = DateTime.Now;
        taskRepository.Update(task);
        await taskRepository.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int id)
    {
        var task = await taskRepository.GetByIdAsync(id);

        if (task == null) return;

        taskRepository.Delete(task);
        await taskRepository.SaveChangesAsync();
    }

    public async Task DeleteUserTasksAsync(UserTasksDelete userTasksDelete)
    {
        await taskRepository.DeleteByUserIdAsync(userTasksDelete.UserId);
    }

    public async Task<TaskGet> GetTaskByIdAsync(int id)
    {
        var tasks = await taskRepository.GetByIdAsync(id);
        return mapper.Map<TaskGet>(tasks);
    }

    public async Task<List<TaskGet>> GetTasksByUserIdAsync(string userId)
    {
        var tasks = await taskRepository.GetByUserIdAsync(userId);
        return mapper.Map<List<TaskGet>>(tasks);
    }
}

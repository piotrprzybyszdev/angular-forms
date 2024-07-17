using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using Microsoft.EntityFrameworkCore;

namespace FormsBackendInfrastructure;

public class TaskRepository(ApplicationDbContext context) : ITaskRepository
{
    readonly DbSet<TaskModel> set = context.Set<TaskModel>();

    public async Task<int> InsertAsync(TaskModel task)
    {
        await set.AddAsync(task);
        return task.Id;
    }

    public async Task Update(TaskModel task)
    {
        set.Attach(task);
        context.Entry(task).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task<List<TaskModel>> GetAsync()
    {
        return await set.ToListAsync();
    }

    public async Task<TaskModel?> GetByIdAsync(int id)
    {
        return await set.Where((TaskModel task) => task.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<List<TaskModel>> GetByUserIdAsync(string userId)
    {
        return await set.Where((TaskModel task) => task.Account.Id == userId)
            .ToListAsync();
    }

    public async Task Delete(TaskModel task)
    {
        if (context.Entry(task).State == EntityState.Detached)
            set.Attach(task);
        set.Remove(task);
        await Task.CompletedTask;
    }

    public async Task DeleteByUserIdAsync(string userId)
    {
        await set.Where((TaskModel task) => task.Account.Id == userId)
            .ForEachAsync(task => 
            {
                if (context.Entry(task).State == EntityState.Detached)
                    set.Attach(task);
                set.Remove(task);
            });
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}

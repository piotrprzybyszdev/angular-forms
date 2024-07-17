using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using Microsoft.EntityFrameworkCore;

namespace FormsBackendInfrastructure;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    readonly DbSet<UserModel> set = context.Set<UserModel>();

    public async Task<int> InsertAsync(UserModel user)
    {
        await set.AddAsync(user);
        return user.Id;
    }

    public async Task UpdateAsync(UserModel user)
    {
        set.Attach(user);
        context.Entry(user).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task<List<UserModel>> GetAsync()
    {
        return await set.ToListAsync();
    }

    public async Task<UserModel?> GetyByIdAsync(int id)
    {
        return await set.Where((UserModel user) => user.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task DeleteAsync(UserModel user)
    {
        if (context.Entry(user).State == EntityState.Detached)
            set.Attach(user);
        set.Remove(user);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}

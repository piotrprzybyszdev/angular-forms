using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FormsBackendInfrastructure;

public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
{
    public async Task<string> InsertAsync(ApplicationUser user)
    {
        await userManager.CreateAsync(user);
        return user.Id;
    }

    public async Task UpdateAsync(ApplicationUser user)
    {
        var model = await userManager.FindByIdAsync(user.Id)
            ?? throw new Exception("user info wasn't validated");

        model.UserName = user.UserName;
        model.Email = user.Email;
        model.FirstName = user.FirstName;
        model.LastName = user.LastName;

        await userManager.UpdateAsync(model);
    }

    public async Task<List<ApplicationUser>> GetAsync()
    {
        return await userManager.Users.ToListAsync();
    }

    public async Task<ApplicationUser?> GetyByIdAsync(string id)
    {
        return await userManager.FindByIdAsync(id);
    }

    public async Task DeleteAsync(ApplicationUser user)
    {
        var model = await userManager.FindByIdAsync(user.Id)
            ?? throw new Exception("user info wasn't validated");
        
        await userManager.DeleteAsync(model);
    }

    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}

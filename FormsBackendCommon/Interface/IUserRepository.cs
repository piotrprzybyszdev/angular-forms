using FormsBackendCommon.Model;

namespace FormsBackendCommon.Interface;

public interface IUserRepository
{
    Task<string> InsertAsync(ApplicationUser user);
    Task UpdateAsync(ApplicationUser user);
    Task<List<ApplicationUser>> GetAsync();
    Task<ApplicationUser?> GetyByIdAsync(string id);
    Task DeleteAsync(ApplicationUser user);
    Task SaveChangesAsync();
}

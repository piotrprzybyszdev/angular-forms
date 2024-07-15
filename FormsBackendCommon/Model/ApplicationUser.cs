using Microsoft.AspNetCore.Identity;

namespace FormsBackendCommon.Model;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public List<TaskModel> Tasks { get; set; } = default!;
}

using DemoProjectCommon.Model;

namespace FormsBackendCommon.Model;

public class UserModel : BaseEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string HashedPassword { get; set; } = default!;
    public string PasswordSalt { get; set; } = default!;
}

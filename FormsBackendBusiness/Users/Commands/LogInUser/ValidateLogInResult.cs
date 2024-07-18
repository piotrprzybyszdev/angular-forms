using FormsBackendBusiness.Users.Queries.GetUsers;

namespace FormsBackendBusiness.Users.Commands.LogInUser;

public class ValidateLogInResult
{
    public UserGet User { get; init; } = default!;
}

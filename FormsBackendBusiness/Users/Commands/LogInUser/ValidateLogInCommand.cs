using MediatR;

namespace FormsBackendBusiness.Users.Commands.LogInUser;

public class ValidateLogInCommand : IRequest<ValidateLogInResult>
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}

using MediatR;

namespace FormsBackendBusiness.Users.Commands.AddUser;

public class AddUserCommand : IRequest<AddUserCommandResult>
{
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}

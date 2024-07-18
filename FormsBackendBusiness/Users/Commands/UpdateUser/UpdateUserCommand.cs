using MediatR;

namespace FormsBackendBusiness.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<UpdateUserCommandResult>
{
    public int Id { get; init; }
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
}

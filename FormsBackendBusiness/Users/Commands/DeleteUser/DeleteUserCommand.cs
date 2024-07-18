using MediatR;

namespace FormsBackendBusiness.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<DeleteUserCommandResult>
{
    public int Id { get; init; }
}

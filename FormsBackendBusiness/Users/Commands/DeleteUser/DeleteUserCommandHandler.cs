using FormsBackendBusiness.Exceptions;
using FormsBackendBusiness.Tasks.Commands.DeleteUserTasks;
using FormsBackendCommon.Interface;
using FormsBackendCommon.Model;
using MediatR;

namespace FormsBackendBusiness.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IGenericRepository<UserModel> userRepository,
    IMediator mediator)
    : IRequestHandler<DeleteUserCommand, DeleteUserCommandResult>
{
    public async Task<DeleteUserCommandResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id) ??
            throw new UserNotFoundException(request.Id);

        await mediator.Send(new DeleteUserTasksCommand() { UserId = request.Id });
        await userRepository.DeleteAsync(user);
        await userRepository.SaveChangesAsync();

        return new DeleteUserCommandResult();
    }
}

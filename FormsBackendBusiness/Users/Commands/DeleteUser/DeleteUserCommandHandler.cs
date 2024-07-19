using FormsBackendBusiness.Exceptions;
using FormsBackendBusiness.Tasks.Commands.DeleteUserTasks;
using Dapper;
using FormsBackendInfrastructure;
using MediatR;

namespace FormsBackendBusiness.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IMediator mediator, ApplicationDbContext dbContext)
    : IRequestHandler<DeleteUserCommand, DeleteUserCommandResult>
{
    public async Task<DeleteUserCommandResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteUserTasksCommand() { UserId = request.Id });
        await DeleteUser(request.Id);

        return new DeleteUserCommandResult();
    }

    public async Task DeleteUser(int Id)
    {
        string sql = @"
DELETE FROM Users WHERE Id = @Id";

        var rowsAffected = await dbContext.Connection.ExecuteAsync(sql, new { Id });
        if (rowsAffected == 0)
            throw new UserNotFoundException(Id);
    }
}

using Dapper;
using FormsBackendInfrastructure;
using MediatR;

namespace FormsBackendBusiness.Tasks.Commands.DeleteUserTasks;

public class DeleteUserTasksCommandHandler(ApplicationDbContext dbContext)
    : IRequestHandler<DeleteUserTasksCommand, DeleteUserTasksCommandResult>
{
    public async Task<DeleteUserTasksCommandResult> Handle(DeleteUserTasksCommand request, CancellationToken cancellationToken)
    {
        await DeleteUserTasks(request.UserId);
        return new DeleteUserTasksCommandResult();
    }

    public async Task DeleteUserTasks(int UserId)
    {
        string sql = @"
DELETE FROM Tasks WHERE UserId = @UserId;";

        await dbContext.Connection.ExecuteAsync(sql, new { UserId });
    }
}

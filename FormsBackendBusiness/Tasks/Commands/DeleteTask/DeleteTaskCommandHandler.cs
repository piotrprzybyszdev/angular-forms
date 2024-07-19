using Dapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendInfrastructure;
using MediatR;

namespace FormsBackendBusiness.Tasks.Commands.DeleteTask;

public class DeleteTaskCommandHandler(ApplicationDbContext dbContext)
    : IRequestHandler<DeleteTaskCommand, DeleteTaskCommandResult>
{
    public async Task<DeleteTaskCommandResult> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        await DeleteTask(request.Id);
        return new DeleteTaskCommandResult();
    }

    public async Task DeleteTask(int Id)
    {
        var sql = @"
DELETE FROM Tasks WHERE Id = @Id;";

        var rowsAffected = await dbContext.Connection.ExecuteAsync(sql, new { Id });
        if (rowsAffected == 0)
            throw new TaskNotFoundException(Id);
    }
}

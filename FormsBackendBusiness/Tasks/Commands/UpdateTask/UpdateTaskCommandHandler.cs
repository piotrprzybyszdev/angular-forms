using Dapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendInfrastructure;
using MediatR;

namespace FormsBackendBusiness.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler(UpdateTaskRequestValidator updateTaskRequestValidator,
    ApplicationDbContext dbContext)
    : IRequestHandler<UpdateTaskCommand, UpdateTaskCommandResult>
{
    public async Task<UpdateTaskCommandResult> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await updateTaskRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        await UpdateTask(request.Id, request.Title, request.Description, request.DueDate, DateTime.Now);

        return new UpdateTaskCommandResult();
    }

    public async Task UpdateTask(int Id, string Title, string Description, DateTime DueDate, DateTime ModificationDate)
    {
        string sql = @"
UPDATE Tasks SET
Title=@Title, Description=@Description, ModificationDate=@ModificationDate, DueDate=@DueDate
WHERE Id=@Id;";

        int affectedRows = await dbContext.Connection.ExecuteAsync(sql, new { Id, Title, Description, DueDate, ModificationDate });
        if (affectedRows == 0)
            throw new TaskNotFoundException(Id);
    }
}

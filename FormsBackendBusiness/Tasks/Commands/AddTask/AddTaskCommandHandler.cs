using Dapper;
using FormsBackendBusiness.Exceptions;
using FormsBackendInfrastructure;
using MediatR;

namespace FormsBackendBusiness.Tasks.Commands.AddTask;

public class AddTaskCommandHandler(ApplicationDbContext dbContext, AddTaskRequestValidator addTaskRequestValidator)
    : IRequestHandler<AddTaskCommand, AddTaskCommandResult>
{
    public async Task<AddTaskCommandResult> Handle(AddTaskCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await addTaskRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid) throw new ValidationFailedException(validationResult.Errors);

        var taskId = await InsertTask(request.Title, request.Description, DateTime.Now, request.DueDate, request.UserId);

        return new AddTaskCommandResult() { Id = taskId };
    }

    private async Task<int> InsertTask(string Title, string Description, DateTime CreationDate, DateTime DueDate, int UserId)
    {
        string sql = @"
INSERT INTO Tasks (Title, Description, CreationDate, ModificationDate, DueDate, UserId)
VALUES (@Title, @Description, @CreationDate, @CreationDate, @DueDate, @UserId);
SELECT last_insert_rowid()";

        return await dbContext.Connection.QuerySingleOrDefaultAsync<int?>(sql, new { Title, Description, CreationDate, DueDate, UserId }) ??
            throw new UserNotFoundException(UserId);
    }
}
